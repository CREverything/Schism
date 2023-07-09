using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassCollision : MonoBehaviour
{
    public Material[] materials;  
    public float windSpeed;
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        foreach (var material in materials)
        {
            material.SetFloat("_WindSpeed", 3);
        }
    }
}
