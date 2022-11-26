using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.X))
      {
        Attack();
      }

    }

    void Attack ()
    {
        // Play an attack animation
        anim.SetTrigger("Attack");

        // Detect all enimes in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Danage enemies 
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        
    }
    void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            return;

            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
}
