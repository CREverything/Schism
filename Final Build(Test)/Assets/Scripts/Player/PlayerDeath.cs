using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;

    public AudioSource Source;
    public AudioClip Clip;

    // Start is called before the first frame update.
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Player collisions with obstacles.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            Die();
        }
    }

    // Player death animation and restart.
    private void Die()
    {
        Source.PlayOneShot(Clip);
        _rb.bodyType = RigidbodyType2D.Static;
        RestartLevel();
    }

    // Restart level functionality.
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

}