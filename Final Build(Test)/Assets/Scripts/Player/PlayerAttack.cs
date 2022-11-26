using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
   Animator anim;
   public float delay = 0.3f;
   private bool attackBlocked;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
     public void Attack()
    {   
        if(Input.GetKeyDown("x"))
        {
            anim.SetTrigger("Attack");
        }
        else 
        {
            anim.SetTrigger("NotAttack");
        }
       
}
}