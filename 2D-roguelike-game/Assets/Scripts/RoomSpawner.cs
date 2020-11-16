using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection; // 1 = bottom , 2 = top , 3 = left , 4 = right
    private RoomTemplates templates;
    private int random;
    private bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>(); // link template to GameObject RoomTemplates
        Invoke("SpawnRooms", 0.1f); // call SpawnRooms function with a small delay, to detect collision of multiple rooms
    }

    void SpawnRooms()
    {
        if(spawned == false){
            // spawn random rooms
            if(openingDirection == 1){
                random = Random.Range(0,templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[random], transform.position, Quaternion.identity);
            } else if(openingDirection == 2){
                random = Random.Range(0,templates.topRooms.Length);
                Instantiate(templates.topRooms[random], transform.position, Quaternion.identity);
            } else if(openingDirection == 3){
                random = Random.Range(0,templates.leftRooms.Length);
                Instantiate(templates.leftRooms[random], transform.position, Quaternion.identity);
            } else if(openingDirection == 4){
                random = Random.Range(0,templates.rightRooms.Length);
                Instantiate(templates.rightRooms[random], transform.position, Quaternion.identity);
            }
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // check if a room is already spawned at that position
        if(other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().spawned){
            Destroy(gameObject);
        }
    }
}
