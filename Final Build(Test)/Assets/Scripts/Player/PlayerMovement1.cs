using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{

    [SerializeField] private LayerMask _jumpableGround;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField]private float _dashingPower = 12f;
    [SerializeField]private float _dashingTime = 0.2f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spr;
    private BoxCollider2D _coll;
    private ParticleSystem.EmissionModule _dustEmission;
    public ParticleSystem ImpactEffect;
    public AudioSource Source1;
    public AudioClip Clip2;
    public AudioClip Clip1;
    public AudioSource Source2;
    public ParticleSystem Dust;
    public ParticleSystem JumpDust;
    public ParticleSystem DashDust;
    public Ghost Ghost;
    public bool IsLoaded;
    public Player player;
    public float OriginalGravity;

    private bool _isGrounded;
    private bool _attack;
    private bool _wasOnGround;
    private bool _doubleJump;
    private bool _isFacingRight = true;
    private float _horizontal;
    private float _speed = 6f;
    private float _jumpingPower = 6f;

    private bool _canDash = true;
    private bool _isDashing;
     private Vector2 _dashingDir;

    KeyCode lastKeyCode;
    private enum _movementState {idle, running, jummping, falling, attack}

    // Start is called before the first frame update.
    private void Start()
    {
        _rb =  GetComponent<Rigidbody2D>();
        _animator =  GetComponent<Animator>();
        _coll = GetComponent<BoxCollider2D>();
        _spr = GetComponent<SpriteRenderer>();
        _attack = false;
        Source1 = GetComponent<AudioSource>();
        Source2 = GetComponent<AudioSource>();

        DashDust.Stop();
        _dustEmission = Dust.emission;
    }

    // Update is called once per frame.
    private void Update(){

        Source1.volume = Random.Range(0f,0.5f);

        // Set double jump to false if the player is not grounded.
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            _doubleJump = false;
        }

        // Horizontal player movement.
        _horizontal = Input.GetAxisRaw("Horizontal");

        // Double jump functionality.
         if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || _doubleJump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpingPower);
                _doubleJump = !_doubleJump;

                //Dust effects.
                JumpDust.Play();
            }
        }

        // Jump functionality.
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f && !_isDashing)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }

        // Recieving attack inputs.
        if (Input.GetKeyDown("x"))
        {
            _attack = true;
        }

        else
        {
            _attack = false;
        }

        // Footstep sound effects.
        if (_rb.velocity.x > 0f && IsGrounded())
        {
            Source1.enabled = true;
        }

        else if (_rb.velocity.x < 0 && IsGrounded())
        {
            Source1.enabled = true;
        }

        else
        {
            Source1.enabled = false;
        }

        // Walking dust effects.
        if(Input.GetAxisRaw("Horizontal") != 0 && IsGrounded())
       {
           _dustEmission.rateOverTime = 35f;
       }

       else
       {
           _dustEmission.rateOverTime = 0f;
       }

         if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
        {
            _isDashing = true;
            _canDash = false;
            _dashingDir = new Vector2(_horizontal, Input.GetAxisRaw("Vertical"));

            if (_dashingDir == Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }


            StartCoroutine(StopDashing());

        }

        if (_isDashing && Input.GetAxis("Horizontal")!=0 || _isDashing && Input.GetAxis("Vertical")!=0)
        {
            Ghost.makeGhost = true;
            _rb.velocity = _dashingDir.normalized * _dashingPower;
            return;
        }

        if(IsGrounded())
        {
            _canDash = true;
        }

        //Update player animations and flip the player sprite.
        UpdateAnimationState();
        Flip();
   }

    // Fixed update is called multiple times per frame.
        private void FixedUpdate()
    {
        if(!_isDashing)
        {
            _rb.velocity = new Vector2(_horizontal * _speed, _rb.velocity.y);
            return;
        }
    }

    // Flip player sprite based on directional orientation.
    private void Flip()
    {
        if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            _isFacingRight = !_isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Update player's animation state.
    private void UpdateAnimationState()
        {
            _movementState state;

        if (_horizontal != 0 && IsGrounded())
        {
            state = _movementState.running;
        }

        else if(_horizontal == 0 && IsGrounded())
        {
            state = _movementState.idle;
        }

        else
        {
            state = _movementState.idle;
        }

        if (_rb.velocity.y > .1f)
        {
            state = _movementState.jummping;
        }

        else if (_rb.velocity.y < - .1f)
        {
            state = _movementState.falling;
        }

        _animator.SetInteger("state", (int)state);
    }

    // Boolean variable to check if the player is grounded.
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(_coll.bounds.center, _coll.bounds.size, 0f, Vector2.down, .1f, _jumpableGround);
    }

   private IEnumerator StopDashing()
    {
         yield return new WaitForSeconds(_dashingTime);
        _isDashing = false;
        _rb.velocity = Vector2.zero;
        _canDash = false;
        Ghost.makeGhost = false;
    }
    // Loads the game.
    public void LoadGame()
   {
       IsLoaded = true;
   }
}
