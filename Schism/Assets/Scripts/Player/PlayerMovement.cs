using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // The speed at which the player moves
    public float speed = 5.0f;

    // The dash speed and duration
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.5f;

    // The jump force and the number of jumps allowed
    public float jumpForce = 10.0f;
    public int maxJumps = 2;

    // The duration of coyote time (in seconds)
    public float coyoteTime = 0.2f;

    // The Rigidbody2D component attached to the player
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;



    // A timer for the dash duration
    private float dashTimer;

    // A flag to check if the player is dashing
    private bool isDashing;

    // A counter for the number of jumps
    private int jumps;

    // A timer for coyote time
    private float coyoteTimer;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Initialize the number of jumps
        jumps = maxJumps;
    }

    void Update()
    {
        // Get the horizontal input axis
        float horizontalInput = Input.GetAxis("Horizontal");

        // Check if the left shift key was pressed
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Set the dash flag to true
            isDashing = true;

            // Reset the dash timer
            dashTimer = dashDuration;
        }

        // Check if the player is dashing
        if (isDashing)
        {
            // Decrement the dash timer
            dashTimer -= Time.deltaTime;

            // Calculate the dash vector
            Vector2 dashVector = new Vector2(horizontalInput, 0) * dashSpeed;

            // Set the player's velocity to the dash vector
            rb.velocity = dashVector;

            // Check if the dash timer has expired
            if (dashTimer <= 0)
            {
                // Set the dash flag to false
                isDashing = false;
            }
        }
        else
        {
            // Calculate the player's movement
            Vector2 movement = new Vector2(horizontalInput, 0) * speed;

            // Set the player's velocity to the movement vector
            rb.velocity = movement;

            /*// Check if the player is on the ground
            if (IsGrounded())
            {
                // Reset the number of jumps
                jumps = maxJumps;

                // Reset the coyote timer
                coyoteTimer = 0;
            }
            else
            {
                // Increment the coyote timer
                coyoteTimer += Time.deltaTime;
            }

            // Check if the player pressed the space key and has jumps remaining
            if (Input.GetKeyDown(KeyCode.Space) && (jumps > 0 || coyoteTimer <= coyoteTime))
            {
                // Apply a jump force to the player
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                // Decrement the number of jumps
                jumps--;

                // Reset the coyote timer
                coyoteTimer = 0;
            }*/
        }
    }

    
}

