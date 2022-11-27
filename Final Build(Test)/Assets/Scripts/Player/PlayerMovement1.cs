using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f; 
    [SerializeField] private float jumpForce = 5f; 
   
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

    KeyCode lastKeyCode;
    

    private enum MovementState {idle, running, jummping, falling, attack}

     private void Start()
    {
        jumpForce = 7f;
        rb =  GetComponent<Rigidbody2D>();
        anim =  GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        Attack = false;
        source1 = GetComponent<AudioSource>();
        clip1 = GetComponent<AudioClip>();
        
        dashDust.Stop();

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
          
            source1.enabled = true;

        }

        else if (rb.velocity.x < 0 && IsGrounded()) 
        {
            
            source1.enabled = true;

        }

        else 
        {
            source1.enabled = false;
        }
        
          if(Input.GetAxisRaw("Horizontal") != 0 && IsGrounded())
       {
           dustEmission.rateOverTime = 35f;
       } else
       {
           dustEmission.rateOverTime = 0f; 
       }
        //Dashing Left
       if (Input.GetKeyDown(KeyCode.A) && canHorizontalDash)
       {

        if (doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
        {
            StartCoroutine(DashHorizontal(-1f));
        }
        else
        {
            doubleTapTime = Time.time + 0.5f;
        }
        lastKeyCode = KeyCode.A;

       }
        //Dashing Right
       if (Input.GetKeyDown(KeyCode.D) && canHorizontalDash)
       {

        if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
        {
            StartCoroutine(DashHorizontal(1f));
        }
        else
        {
            doubleTapTime = Time.time + 0.5f;
        }
        lastKeyCode = KeyCode.D;
       }
        //Dash Left
        if (Input.GetKeyDown(KeyCode.LeftArrow) && canHorizontalDash)
        {
        if (doubleTapTime > Time.time && lastKeyCode == KeyCode.LeftArrow)
        {
            StartCoroutine(DashHorizontal(-1f));
        }
        else
        {
            doubleTapTime = Time.time + 0.5f;
        }
        lastKeyCode = KeyCode.LeftArrow;
        
       }
        //Dashing Right
       if (Input.GetKeyDown(KeyCode.RightArrow) && canHorizontalDash)
       {

        if (doubleTapTime > Time.time && lastKeyCode == KeyCode.RightArrow)
        {
            StartCoroutine(DashHorizontal(1f));
        }
        else
        {
            doubleTapTime = Time.time + 0.5f;
        }
        lastKeyCode = KeyCode.RightArrow;
       }

       //Dashing Up
       if (Input.GetKeyDown(KeyCode.UpArrow) && canVerticalDash)
       {

        if (doubleTapTime > Time.time && lastKeyCode == KeyCode.UpArrow)
        {
            StartCoroutine(DashVertical(1f));
        }
        else
        {
            doubleTapTime = Time.time + 0.5f;
        }
        lastKeyCode = KeyCode.UpArrow;
       }


        UpdateAnimationState();
        
        Flip();


   }
    
        private void FixedUpdate()
    {
        if (!isDashing || !isDashing){
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        rb.gravityScale = 1f;
        }
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

    IEnumerator DashHorizontal (float directionX) 
    {
        canHorizontalDash = false;
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistanceX * directionX, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        dashDust.Play();
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
        rb.gravityScale = gravity;
        dashDust.Stop();
        yield return new WaitForSeconds(dashingHorizontalCooldown);
        canHorizontalDash = true;


    }
    IEnumerator DashVertical (float directionY) 
    {
        canVerticalDash = false;
        isDashing = true;
        rb.velocity = new Vector2(0f, rb.velocity.y);
        rb.AddForce(new Vector2(0f, dashDistanceY * directionY), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        dashDust.Play();
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
        rb.gravityScale = gravity;
        dashDust.Stop();
        yield return new WaitForSeconds(dashingVerticalCooldown);
        canVerticalDash = true;
    }
}

