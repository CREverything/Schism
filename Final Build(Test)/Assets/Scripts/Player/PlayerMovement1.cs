using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{

    [SerializeField] private LayerMask _jumpableGround;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;
    private BoxCollider2D coll;
    public AudioSource source1;
    public AudioClip clip1;
    public AudioSource source2;
    public AudioClip clip2;
    public ParticleSystem dust;
    public ParticleSystem jumpDust;
    public ParticleSystem dashDust;
    private ParticleSystem.EmissionModule dustEmission;
    public ParticleSystem impactEffect;
    public Ghost ghost;

    private bool isGrounded;
    private bool Attack;
    private bool wasOnGround;
    private bool isDashing;
    private bool doubleJump;
    private bool isFacingRight = true;
    private bool canHorizontalDash = true;
    private bool canVerticalDash = true;
    private float horizontal;
    private float speed = 6f;
    private float jumpingPower = 6f;
    private float dashDistanceX = 15f;
    private float dashDistanceY = 6f;
    private float doubleTapTime;
    private float dashingTime = 0.2f;
    private float dashingHorizontalCooldown = 1f;
    private float dashingVerticalCooldown = 1f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spr;
    private BoxCollider2D _coll;
    private ParticleSystem.EmissionModule _dustEmission;
    private bool _isGrounded;
    private bool _attack;
    private bool _wasOnGround;
    private bool _isDashing;
    private bool _doubleJump;
    private bool _isFacingRight = true;
    private bool _canHorizontalDash = true;
    private bool _canVerticalDash = true;
    private float _horizontal;
    private float _speed = 6f;
    private float _jumpingPower = 6f;
    private float _dashDistance = 15f;
    private float _doubleTapTime;
    private float _dashingTime = 0.2f;
    private float _dashingCooldown = 1f;
    private enum _movementState {idle, running, jummping, falling, attack}

    public ParticleSystem ImpactEffect;
    public AudioSource Source1;
    public AudioClip Clip1;
    public AudioSource Source2;
    public AudioClip Clip2;
    public ParticleSystem Dust;
    public ParticleSystem JumpDust;
    public ParticleSystem DashDust;
        KeyCode lastKeyCode;

    // Start is called before the first frame update.
    private void Start()
    {
        _jumpForce = 7f;
        _rb =  GetComponent<Rigidbody2D>();
        _animator =  GetComponent<Animator>();
        _coll = GetComponent<BoxCollider2D>();
        _spr = GetComponent<SpriteRenderer>();
        _attack = false;
        Source1 = GetComponent<AudioSource>();
        Clip1 = GetComponent<AudioClip>();

        DashDust.Stop();
        _dustEmission = Dust.emission;
    }

    // Update is called once per frame.
    private void Update(){

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
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f)
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
        //Dashing Left
       if (Input.GetKeyDown(KeyCode.A) && _canHorizontalDash)

    //Player dash calling.

        //Dashing left.
       if (Input.GetKeyDown(KeyCode.A) && _canHorizontalDash)
       {

        if (_doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
        {
            StartCoroutine(DashHorizontal(-1f));
        }
        else
        {
            _doubleTapTime = Time.time + 0.5f;
        }

        lastKeyCode = KeyCode.A;
       }
        //Dashing Right
       if (Input.GetKeyDown(KeyCode.D) && _canHorizontalDash)
       {

        //Dashing right.
       if (Input.GetKeyDown(KeyCode.D) && _canHorizontalDash)
       {
        if (_doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
        {
            StartCoroutine(DashHorizontal(1f));
        }

        else
        {
            _doubleTapTime = Time.time + 0.5f;
        }
        lastKeyCode = KeyCode.D;
       }
        //Dash Left
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _canHorizontalDash)

        //Dash left V2.
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _canHorizontalDash)
        {
        if (_doubleTapTime > Time.time && lastKeyCode == KeyCode.LeftArrow)
        {
            StartCoroutine(DashHorizontal(-1f));
        }

        else
        {
            _doubleTapTime = Time.time + 0.5f;
        }
        lastKeyCode = KeyCode.LeftArrow;

       }
        //Dashing Right
       if (Input.GetKeyDown(KeyCode.RightArrow) && _canHorizontalDash)
       {


        lastKeyCode = KeyCode.LeftArrow;
       }

        //Dashing right V2.
       if (Input.GetKeyDown(KeyCode.RightArrow) && _canHorizontalDash)
       {
        if (_doubleTapTime > Time.time && lastKeyCode == KeyCode.RightArrow)
        {
            StartCoroutine(DashHorizontal(1f));
        }

        else
        {
            _doubleTapTime = Time.time + 0.5f;
        }
        lastKeyCode = KeyCode.RightArrow;
       }

       //Dashing Up
       if (Input.GetKeyDown("up") && _canVerticalDash)
       {

        if (doubleTapTime > Time.time)
        {
            StartCoroutine(DashVertical(1f));
        }
        else
        {
            doubleTapTime = Time.time + 0.5f;
        }

       }

       //Dash Down
       if (Input.GetKeyDown("down") && _canVerticalDash)
       {

        if (doubleTapTime > Time.time)
        {
            StartCoroutine(DashVertical(-1f));
        }
        else
        {
            doubleTapTime = Time.time + 0.5f;
        }

       }
        lastKeyCode = KeyCode.RightArrow;
       }

        //Update player animations and flip the player sprite.
        UpdateAnimationState();
        Flip();
   }

    // Fixed update is called multiple times per frame.
        private void FixedUpdate()
    {
        if (!isDashing || !isDashing){
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        rb.gravityScale = 1f;
        if (!_isDashing)
        {
        _rb.velocity = new Vector2(_horizontal * _speed, _rb.velocity.y);
        _rb.gravityScale = 1f;
        }
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

        if (_rb.velocity.x > 0f)
        {
            state = _movementState.running;
        }

        else if (_rb.velocity.x < 0)
        {
            state = _movementState.running;
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

    IEnumerator DashHorizontal (float directionX)
    {
        canHorizontalDash = false;
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistanceX * directionX, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        dashDust.Play();
        ghost.makeGhost = true;
        yield return new WaitForSeconds(0.3f);
        isDashing = false;
        rb.gravityScale = gravity;
        dashDust.Stop();
        ghost.makeGhost = false;
        yield return new WaitForSeconds(dashingHorizontalCooldown);
        canHorizontalDash = true;
    }

    // Dash preferences.
    IEnumerator DashVertical(float directionY)
    {
        canVerticalDash = false;
        isDashing = true;
        rb.velocity = new Vector2(0f, rb.velocity.y);
        rb.AddForce(new Vector2(0f, dashDistanceY * directionY), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        dashDust.Play();
        ghost.makeGhost = true;
        yield return new WaitForSeconds(0.3f);
        isDashing = false;
        rb.gravityScale = gravity;
        dashDust.Stop();
        ghost.makeGhost = false;
        yield return new WaitForSeconds(dashingVerticalCooldown);
        canVerticalDash = true;
    }
}


