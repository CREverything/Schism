using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinC : MonoBehaviour
{
    private int Enemy = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")){

            Destroy(collision.gameObject);
            Enemy++;

        }

    }

     
}
