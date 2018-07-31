using System.Collections;
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
    GameObject floorParent;

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
        StartCoroutine(GenerateFloor());
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
