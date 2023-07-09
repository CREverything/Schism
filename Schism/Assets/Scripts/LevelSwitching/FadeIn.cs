using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour

{
public GameObject obj;
    private bool canFade;
    private Color alphaColor;
    private float timeToFade = 1.0f;
     
    public void Start()
    {
        canFade = false;
        alphaColor = obj.GetComponent<MeshRenderer>().material.color;
        alphaColor.a = 0;
    }
    public void Update()
    {
        if (canFade)
        {
        obj.GetComponent<MeshRenderer>().material.color = Color.Lerp(obj.GetComponent<MeshRenderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
        }
    }
}
