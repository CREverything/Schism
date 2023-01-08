using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{
    // The prefab to instantiate
    public GameObject prefab;


    void Update()
    {
        // Check if the X key was pressed
        if (Input.GetKeyDown(KeyCode.X))
        {

            // Instantiate the prefab at the player's position
            Instantiate(prefab, transform.position, Quaternion.identity);
        
            // Delete the player game object
            Destroy(gameObject);
        }
        
    }
}