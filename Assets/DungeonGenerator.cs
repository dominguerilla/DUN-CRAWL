using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {

    public int numberOfRooms = 3;
    public int roomPlacementRetries = 10;
    public float minDistanceBetweenRooms = 4.0f;

    public float minRoomWidth = 5f;
    public float maxRoomWidth = 10f;

    public float minRoomLength = 5f;
    public float maxRoomLength = 10f;

    public Transform topLeftCorner, bottomRightCorner;

    private List<GameObject> rooms;

	// Use this for initialization
	void Start () {
        if(!topLeftCorner || !bottomRightCorner) {
            Debug.LogError("Maze corners not assigned!");
            Destroy(this);
        }

        rooms = new List<GameObject>();
        StartCoroutine(GenerateRooms());
	}

	IEnumerator GenerateRooms() {
		for(int i = 0; i < numberOfRooms; i++) {
            for(int tries = 0; tries < roomPlacementRetries; tries++) {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bool placed = TryPlacingRoom(obj); 
                if(placed) {
                    rooms.Add(obj);
                    break;
                }
            yield return null;
            }
        }
        Debug.Log("Rooms placed: " + rooms.Count);
    }

    bool TryPlacingRoom(GameObject obj) {
            // Size
            float xSize = Random.Range(minRoomWidth, maxRoomWidth);
            float zSize = Random.Range(minRoomLength, maxRoomLength);
            obj.transform.localScale = new Vector3(xSize, 1f, zSize);

            // Position
            float X = Random.Range(topLeftCorner.position.x, bottomRightCorner.position.x);
            float Z = Random.Range(topLeftCorner.position.z, bottomRightCorner.position.z);
            obj.transform.position = new Vector3(X, 0f, Z);

            foreach(GameObject room in rooms) {
                Collider newRoom = obj.GetComponent<Collider>();
                Collider existingRoom = room.GetComponent<Collider>();
                
                // get closest point between the two
                Vector3 newRoomClosestPoint = newRoom.bounds.ClosestPoint(room.transform.position);
                Vector3 oldRoomClosestPoint = existingRoom.bounds.ClosestPoint(newRoom.transform.position);

                if(newRoom.bounds.Intersects(existingRoom.bounds) || Vector3.Distance(newRoomClosestPoint, oldRoomClosestPoint) < minDistanceBetweenRooms) {
                    Destroy(obj);
                    return false;
                }
            }
            
            return true;

    }

	// Update is called once per frame
	void Update () {
		
	}
}
