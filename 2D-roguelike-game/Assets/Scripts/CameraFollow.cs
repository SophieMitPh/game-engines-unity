using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraDistance = 30.0f;

    void Awake() 
    {
        GetComponent<UnityEngine.Camera>().orthographicSize = ((Screen.height / 2) / cameraDistance);
    }

    void FixedUpdate()
    {
        transform.position = new Vector2(player.position.x, player.position.y);
    }
}
