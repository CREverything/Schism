using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StressBar : MonoBehaviour
{
     public Slider stressBar;
     public float gameTime;
     private bool stopTimer;
    public bool Restartlevel;

   
    
    void Start()
    {
        stopTimer = false;
        //stressBar.maxValue = gameTime;
        stressBar.value = gameTime;
        stressBar.maxValue = gameTime;
        Restartlevel = false;
        

    }
    
    void Update()
    {
        float time = gameTime -= Time.deltaTime;
        
        int min = Mathf.FloorToInt(time/60);
        int sec = Mathf.FloorToInt(time - min * 60); 

        if (time <= 0)
        {
            stopTimer = true; 
            Restartlevel = true;
            stressBar.value = 15f;
            stressBar.gameObject.SetActive(false);

        }

        if (stopTimer == false)
        {
            stressBar.value = time;
            
        }

        if (Restartlevel == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            stressBar.value = 1;
            Debug.Log("");
        }
    }
}

