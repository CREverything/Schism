using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWall;

    private float dirX;
    [SerializeField] private float moveSpeed = 7f; 
    [SerializeField] private float jumpForce = 5f; 
    private bool Attack; 
    public AudioSource source;
    public AudioClip clip;
    public ParticleSystem dust;
    private ParticleSystem.EmissionModule dustEmission;
    public ParticleSystem impactEffect;
    private bool wasOnGround;
    private float wallJumpCooldown;
    
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
        
        
         dirX = Input.GetAxis("Horizontal");        

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        
        
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

        //Wall Jumping
        if(wallJumpCooldown > 0.2f)
        {
            if (Input.GetButtonDown("Jump"))
            {
              Jump();
            }

            if(OnWall() && !IsGrounded())
            {
                rb.gravityScale = 0;
                //rb.velocity = Vector2.zero;
            }
            else
            {
                rb.gravityScale = 1;
            }

        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
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

   private void Jump()
   {
        if(IsGrounded())
        {
            rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
        }
        else if (OnWall() && !IsGrounded())
        {
            if(dirX == 0)
            {
                rb.velocity = new Vector2 (-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.velocity = new Vector2 (-Mathf.Sign(transform.localScale.x) * 3, jumpForce);
            }
            wallJumpCooldown = 0;
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, new Vector2(transform.localScale.x - 1, 0), .1f, jumpableWall);
        return raycastHit.collider != null;
    }

}
