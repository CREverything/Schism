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
    [SerializeField] private LayerMask jumpableWall;
    public int extraJumps;

    private float dirX;
    [SerializeField] private float moveSpeed = 7f; 
    public float jumpForce = 16f; 
    private bool Attack; 
    public AudioSource source;
    public AudioClip clip;
    public ParticleSystem dust;
    public ParticleSystem jumpDust;
    private ParticleSystem.EmissionModule dustEmission;
    public ParticleSystem impactEffect;
    private bool wasOnGround;

    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;

    
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

         if(IsGrounded() == true)
        {
            extraJumps = 1;
        }
        
         dirX = Input.GetAxis("Horizontal");        

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        
        if (Input.GetButtonDown("Jump") && extraJumps > 0 && rb.velocity.y < 2)
        {
        rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
        jumpDust.Play();
        extraJumps--;

        if(extraJumps > 1)
        {
            jumpForce = 14f;
        }
        else
        {
            jumpForce = 7f;
        }

        }
        
        if (Input.GetKeyDown("x"))
        {
            Attack = true;
        }
        else 
        {
            Attack = false;
        }

         if (dirX > 0f && IsGrounded())
        {
          
            source.enabled = true;

        }

        else if (dirX < 0 && IsGrounded()) 
        {
            
            source.enabled = true;

        }

        else 
        {
            source.enabled = false;
        }
        

        UpdateAnimationState();

          if(Input.GetAxisRaw("Horizontal") != 0 && IsGrounded())
       {
           dustEmission.rateOverTime = 35f;
       } else
       {
           dustEmission.rateOverTime = 0f; 
       }
   }
    

    private void UpdateAnimationState()
        {

            MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            spr.flipX = false;
        }

        else if (dirX < 0) 
        {
            state = MovementState.running;            
            spr.flipX = true;
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

    private bool IsWalled()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, new Vector2(transform.localScale.x - 1, 0), .1f, jumpableWall);
    }
}

