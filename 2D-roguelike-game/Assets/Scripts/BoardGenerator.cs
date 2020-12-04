using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public static GameObject[,] roomArray; //  = new GameObject[10,10]
    private int xSpawnRoom;
    private int ySpawnRoom;
    public GameObject spawn;
    public GameObject end;
    private RoomTemplates templates;
    private int random;
    private int columns;
    private int rows;

    // Start is called before the first frame update
    void Start()
    {
        columns = 12;
        rows = 8;

        Debug.Log("columns: " + columns);
        Debug.Log("rows: " + rows);

        xSpawnRoom = 4;
        ySpawnRoom = 2;

        roomArray = new GameObject[columns, rows];
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>(); // link template to GameObject RoomTemplates
        roomArray[xSpawnRoom, ySpawnRoom] = Instantiate(spawn, new Vector3(xSpawnRoom, ySpawnRoom) * 100, Quaternion.identity);

        generateRooms(xSpawnRoom, ySpawnRoom, roomArray, spawn.GetComponent<Room>().openingDirection);
        fillEmptySpots(roomArray, columns, rows);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generateRooms(int x, int y, GameObject[,] roomArray, int[] currRoomDir)
    {
        Debug.Log("----------------");
        Debug.Log("x: " + x);
        Debug.Log("y: " + y);
        Debug.Log("----------------");
        for (int i = 0; i < currRoomDir.Length; i++)
        {
            switch (currRoomDir[i])
            {
            case 2: // bottom
                if(x < 0 || y+1 < 0 || x >= columns || y+1 >= rows){
                    Debug.Log("Out of bounce! Creating End-Wall.");
                    Instantiate(end, new Vector3(x, y+1) * 100, Quaternion.identity);
                }
                else if(roomArray[x,y+1] != null){
                    Debug.Log("There is already a room in this location! x: " + x + " y: " + (y+1) + " room: " + roomArray[x,y+1]);
                }else{
                    random = Random.Range(0,templates.bottomRooms.Length);
                    roomArray[x, y+1] = Instantiate(templates.bottomRooms[random], new Vector3(x, y+1) * 100, Quaternion.identity);
                    Debug.Log(roomArray[x, y+1]);
                    if(roomArray[x, y+1].GetComponent<Room>().openingDirection.Length > 1){
                        generateRooms(x, y+1, roomArray, roomArray[x, y+1].GetComponent<Room>().openingDirection);
                    }
                }
                break;
            case 1: // top
                if(x < 0 || y-1 < 0 || x >= columns || y-1 >= rows){
                    Debug.Log("Out of bounce! Creating End-Wall.");
                    Instantiate(end, new Vector3(x, y-1) * 100, Quaternion.identity);
                }
                else if(roomArray[x,y-1] != null){
                    Debug.Log("There is already a room in this location! x: " + x + " y: " + (y-1) + " room: " + roomArray[x,y-1]);
                }else{
                    random = Random.Range(0,templates.topRooms.Length);
                    roomArray[x, y-1] = Instantiate(templates.topRooms[random], new Vector3(x, y-1) * 100, Quaternion.identity);
                    Debug.Log(roomArray[x, y-1]);
                    if(roomArray[x, y-1].GetComponent<Room>().openingDirection.Length > 1){
                        generateRooms(x, y-1, roomArray, roomArray[x, y-1].GetComponent<Room>().openingDirection);
                    }
                }
                break;
            case 4: // left
                if(x+1 < 0 || y < 0 || x+1 >= columns || y >= rows){
                    Debug.Log("Out of bounce! Creating End-Wall.");
                    Instantiate(end, new Vector3(x+1, y) * 100, Quaternion.identity);
                }
                else if(roomArray[x+1,y] != null){
                    Debug.Log("There is already a room in this location! x: " + (x+1) + " y: " + y + " room: " + roomArray[x+1,y]);
                }else{
                    random = Random.Range(0,templates.leftRooms.Length);
                    roomArray[x+1, y] = Instantiate(templates.leftRooms[random], new Vector3(x+1, y) * 100, Quaternion.identity);
                    Debug.Log(roomArray[x+1, y]);
                    if(roomArray[x+1,y].GetComponent<Room>().openingDirection.Length > 1){
                        generateRooms(x+1, y, roomArray, roomArray[x+1, y].GetComponent<Room>().openingDirection);
                    }
                }
                break;
            case 3: // right
                if(x-1 < 0 || y < 0 || x-1 >= columns || y >= rows){
                    Debug.Log("Out of bounce! Creating End-Wall.");
                    Instantiate(end, new Vector3(x-1, y) * 100, Quaternion.identity);
                }
                else if(roomArray[x-1,y] != null){
                    Debug.Log("There is already a room in this location! x: " + (x-1) + " y: " + y + " room: " + roomArray[x-1,y]);
                }else{
                    random = Random.Range(0,templates.rightRooms.Length);
                    roomArray[x-1, y] = Instantiate(templates.rightRooms[random], new Vector3(x-1, y) * 100, Quaternion.identity);
                    Debug.Log(roomArray[x-1, y]);
                    if(roomArray[x-1,y].GetComponent<Room>().openingDirection.Length > 1){
                        generateRooms(x-1, y, roomArray, roomArray[x-1, y].GetComponent<Room>().openingDirection);
                    }
                }
                break;
            }
        }
    }

    private void fillEmptySpots(GameObject[,] roomArray, int columns, int rows)
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if(roomArray[x,y] != null){
                        // at this position is already a room
                } else{
                        Instantiate(end, new Vector3(x, y) * 100, Quaternion.identity);
                }
            }
        }
    }
}
