using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer(GameObject gameObject)
    {
        PlayerData data = SaveSystem.LoadPlayer();

        Vector2 position;
        position.x = data.Position[0];
        position.y = data.Position[1];
        
        gameObject.transform.position = position;
    }
}
