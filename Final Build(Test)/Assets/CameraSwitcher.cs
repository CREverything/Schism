using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    // The smoothing speed
    public float smoothing = 5.0f;

    // The offset of the camera from the target
    private Vector2 offset;

    void OnDestroy()
    {
        // Get the player objects in the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Check if there are any player objects
        if (players.Length > 0)
        {
            // Calculate the initial offset
            offset = Camera.main.transform.position - players[0].transform.position;

            // Smoothly move the camera towards the first player object's position
            Camera.main.transform.position = Vector2.SmoothDamp(Camera.main.transform.position, players[0].transform.position + (Vector3)offset, ref offset, smoothing * Time.deltaTime);
        }
    }
}
