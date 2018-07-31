﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Blind Digger
/// </summary>
public class AgentGenerate1 : MonoBehaviour {
    
    /*
    agent starts at random position
    changeDirChance = 0% ; placeRoomChance = 0%
    Until enough:
        pick a random direction to go
        go in that direction till you hit a non-room/non-corridor tile
        if(placeRoom(placeRoomChance))
            createRoom()
            placeRoomChance = 0%
            continue
        else
            placeRoomChance += 5%
        if(changeDir(changeDirChance))
            changeDirection()
            changeDirChance = 0%
            continue
        else
            changeDirChance += 5%
    */
    [SerializeField]
    Transform anchor;
    List<GameObject> floorPieces;
    GameObject[,] grid;
    GameObject floorParent;
    GameObject firstFloor = null;

    public GameObject floorPrefab;
    public int numberOfRooms;
    public int width = 50;
    public int length = 50;
	
    
    // Use this for initialization
	void Start () {
	    if(!anchor) {
            Debug.LogError("No anchor set!");
            Destroy(this);
        }

        floorPieces = new List<GameObject>();
        grid = new GameObject[width,length];
        floorParent = new GameObject("Floor Pieces");
        GenerateDungeon();
    }

    void GenerateDungeon(){
        // start in the center
        Vector2 currentPos = new Vector2(width/2, length/2);

        float changeDirChance = 0;
        int placedRoomsNum = 0;
        int direction = pickDirection(100f);
        GameObject currentFloor = PlaceFloor(width/2, length/2);
        Debug.Log("Starting in direction " + direction);

        do{
            int pastDirection = direction;
            direction = pickDirection(changeDirChance, direction);
            
            if(pastDirection == direction){
                changeDirChance += 5f;
                Debug.Log("changeDirChance now at " + changeDirChance + "%");
            }else{
                changeDirChance = 0.0f;
                Debug.Log("changeDirChance reset to " + changeDirChance + "%");
            }
            
            currentFloor = AddFloor(currentFloor, direction);
            
            placedRoomsNum += 1;
        }while (placedRoomsNum < numberOfRooms);
    }
    
    /// <summary>
    /// Instantiates a FloorPrefab GameObject in the space on the grid specified by x,y. Returns the newly made floor.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    GameObject PlaceFloor(int x, int y){
        if(!firstFloor){
            firstFloor = GameObject.Instantiate<GameObject>(floorPrefab);
            floorPieces.Add(firstFloor);
            firstFloor.transform.parent = floorParent.transform;

            BoxCollider firstBox = firstFloor.GetComponent<BoxCollider>();
            Vector3 topLeftCornerOfFloor = anchor.transform.TransformPoint(firstBox.center - new Vector3(-firstBox.size.x / 2, 0, firstBox.size.z / 2));
            firstFloor.transform.position = topLeftCornerOfFloor;
        }

        GameObject floor = GameObject.Instantiate<GameObject>(floorPrefab);
        floorPieces.Add(floor);
        floor.transform.parent = floorParent.transform;

        // calculate floor position
        BoxCollider box = floor.GetComponent<BoxCollider>();
        Vector3 newTilePosition  = firstFloor.transform.position + new Vector3(box.size.x * x, 0f, -box.size.z * y);

        floor.transform.position = newTilePosition;
        return floor;
    }

    GameObject AddFloor(GameObject previousFloor, int direction){
        Vector3 nextLocation;
        
        BoxCollider box = previousFloor.GetComponent<BoxCollider>();
        switch(direction){
            case 1:
                // upwards
                nextLocation = new Vector3(0f, 0f, box.size.z);
                break;
            case 2:
                // to the right
                nextLocation = new Vector3(box.size.x, 0f, 0f);
                break;
            case 3:
                // downwards
                nextLocation = new Vector3(0f, 0f, box.size.z);
                break;
            case 4:
                // to the left
                nextLocation = new Vector3(box.size.x, 0f, 0f);
                break;
            default:
                Debug.LogError("invalid direction specified");
                return null;
        }

        GameObject floor = GameObject.Instantiate<GameObject>(floorPrefab);
        floorPieces.Add(floor);
        floor.transform.parent = floorParent.transform;

        floor.transform.position = previousFloor.transform.position + nextLocation;
        return floor;
    }
    /// <summary>
    /// Will return the given direction (between 1 and 4), but with the 'CHANCE' percent of returning a completely different direction
    /// If you don't pass a currentDirection, there's a chance that this will return the same direction despite activating the direction change.
    /// </summary>
    /// <param name="chance"></param>
    /// <returns></returns>
    int pickDirection(float chance, int currentDirection = 0){
        float percent = chance / 100f;
        float roll = Random.Range(0, 1.0f);
        if(roll <= percent){
            // return different direction
            int direction;
            do{
                direction = (int)Random.Range(1,4);
            }while(direction == currentDirection);

            Debug.Log("Changing direction to " + direction);
            return direction;
        }else{
            // return same direction
            Debug.Log("Maintaining direction " + currentDirection);
            return currentDirection;
        }
    }

    IEnumerator GenerateFloor() {
        floorParent = new GameObject("Floor Pieces");
        GameObject firstFloor = GameObject.Instantiate<GameObject>(floorPrefab);
        floorPieces.Add(firstFloor);
        firstFloor.transform.parent = floorParent.transform;

        BoxCollider firstBox = firstFloor.GetComponent<BoxCollider>();
        Vector3 topLeftCornerOfFloor = anchor.transform.TransformPoint(firstBox.center - new Vector3(-firstBox.size.x / 2, 0, firstBox.size.z / 2));
        firstFloor.transform.position = topLeftCornerOfFloor; 

        //yield return new WaitForSeconds(0.1f);

        for(int z = 0; z < length; z++) {
            for(int x = 0; x < width; x++) {
                //skip the first floor piece, since we already made it
                if(x == 0 && z == 0)
                    continue;
                GameObject currentFloor = GameObject.Instantiate<GameObject>(floorPrefab);
                currentFloor.transform.position = firstFloor.transform.position + new Vector3(firstBox.size.x * x, 0, -firstBox.size.z * z);
                int rotation = Random.Range(0, 4);
                currentFloor.transform.Rotate(currentFloor.transform.up, 90 * rotation);
                floorPieces.Add(currentFloor);
                currentFloor.transform.parent = floorParent.transform;
                //yield return new WaitForSeconds(0.1f);
            }
        }

        yield return null;
    }
}