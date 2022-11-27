using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator Animator;
    public Transform AttackPoint;
    public LayerMask EnemyLayers;
    public float AttackRange = 0.5f;
    public int AttackDamage = 40;

    // Update is called once per frame.
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.X))
      {
        Attack();
      }

    }

    // Attack is called to change animation and check collision.
    void Attack ()
    {
        // Play an attack animation.
        Animator.SetTrigger("Attack");

        // Detect all enimes in range of attack.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

        // Damage enemies. 
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(AttackDamage);
        }

        
    }
    void OnDrawGizmosSelected()
        {
            if (AttackPoint == null)
            return;

            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
        }
}
