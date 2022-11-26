using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f; 
    [SerializeField] private float jumpForce = 5f; 
    //[SerializeField] private TrailRenderer tr;

    public AudioSource source;
    public AudioClip clip;
    public ParticleSystem dust;
    public ParticleSystem jumpDust;
    private ParticleSystem.EmissionModule dustEmission;
    public ParticleSystem impactEffect;
    private bool isGrounded;
    private bool Attack; 
    private bool wasOnGround;
    private float horizontal;
    private float speed = 6f;
    private float jumpingPower = 6f;
    private bool isFacingRight = true;
    private bool doubleJump;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private enum MovementState {idle, running, jummping, falling, attack}

     private void Start()
    {
        jumpForce = 7f;
        rb =  GetComponent<Rigidbody2D>();
        anim =  GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        Attack = false;
        source = GetComponent<AudioSource>();
        clip = GetComponent<AudioClip>();
        

        dustEmission = dust.emission;

    }

    // Update is called once per frame
    private void Update(){
        
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
         
        horizontal = Input.GetAxisRaw("Horizontal");

         if (Input.GetButtonDown("Jump"))
        {

            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                doubleJump = !doubleJump;

                jumpDust.Play();
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        if (Input.GetKeyDown("x"))
        {
            Attack = true;
        }
        else 
        {
            Attack = false;
        }

         if (rb.velocity.x > 0f && IsGrounded())
        {
          
            source.enabled = true;

        }

        else if (rb.velocity.x < 0 && IsGrounded()) 
        {
            
            source.enabled = true;

        }

        else 
        {
            source.enabled = false;
        }
        

        UpdateAnimationState();
        
        Flip();

          if(Input.GetAxisRaw("Horizontal") != 0 && IsGrounded())
       {
           dustEmission.rateOverTime = 35f;
       } else
       {
           dustEmission.rateOverTime = 0f; 
       }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
   }
    
        private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void UpdateAnimationState()
        {

            MovementState state;

        if (rb.velocity.x > 0f)
        {
            state = MovementState.running;
        }

        else if (rb.velocity.x < 0) 
        {
            state = MovementState.running;            
        }

        else 
        {
            state = MovementState.idle;  
        }
        
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jummping;
        }

        else if (rb.velocity.y < - .1f)
        {
            state = MovementState.falling;
        }
        

        anim.SetInteger("state", (int)state);
        
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

     private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        //tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}

