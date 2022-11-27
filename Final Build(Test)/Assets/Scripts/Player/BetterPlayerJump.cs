using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPlayerJump : MonoBehaviour
{
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    Rigidbody2D rb;

    // Start is called before the first frame update.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame.
    void Update()
    {
        // Increases gravity scale when Rigid Body is falling downwards.
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (FallMultiplier - 1) * Time.deltaTime;
        }
    }
}
