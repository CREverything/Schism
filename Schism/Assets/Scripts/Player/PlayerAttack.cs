using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   public float Delay = 0.3f;

   private bool _attackBlocked;

   Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Player attack.
     public void Attack()
    {   
        if(Input.GetKeyDown("x"))
        {
            animator.SetTrigger("Attack");
        }
        else 
        {
            animator.SetTrigger("NotAttack");
        }
       
}
}