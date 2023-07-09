using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play hurt animaion

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;  
        this.enabled = false;
        }
  
    public void OnCollisonEnter2d(Collision2D collision) 
    {
        Destroy(collision.gameObject);
    }
}
