using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clip;

    void Start()
    {
        source = GetComponent<AudioSource>();
        clip = GetComponent<AudioClip>();

    }

    // Update is called once per frame
    void Update()
    {
    

    }
}
