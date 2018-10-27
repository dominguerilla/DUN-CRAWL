using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour {

    public GameObject tilePrefab;
    public int gridWidth, gridLength = 100;

    public int numberOfRooms = 10;
    public int roomPlacementRetries = 50;

    public int minRoomWidth = 5;
    public int maxRoomWidth = 10;

    public int minRoomLength = 5;
    public int maxRoomLength = 10;

    Grid grid;
    List<GameObject> rooms;
    GameObject roomsObject;
    
    public void GenerateRooms(Grid grid) {
        this.grid = grid;

        if (maxRoomWidth >= gridWidth || maxRoomLength >= gridLength) {
            Debug.LogError("Invalid maxRoomWidth/Length; must be less than width/height!");
            Destroy(this);
        }

        rooms = new List<GameObject>();
        roomsObject = new GameObject("Rooms");
        int numRooms = 0;
        for(int tries = 0; tries < roomPlacementRetries && numRooms < numberOfRooms; tries++) {

            int randomX = Random.Range(0, gridWidth - 1);
            int randomZ = Random.Range(0, gridLength - 1);

            int randomWidth = Random.Range(minRoomWidth, maxRoomWidth);
            int randomHeight = Random.Range(minRoomLength, maxRoomLength);

            GridCell currentCell = grid.GetCell(randomX, randomZ);
            if(canPlaceRoom(currentCell, randomWidth, randomHeight)){
                PlaceRoom(currentCell, randomWidth, randomHeight);
                numRooms++;
            }else {
                continue;
            }
        }
        Debug.Log("Finished placing " + numRooms + " rooms.");
    }

    void PlaceRoom(GridCell startingCell, int width, int length) {
        int startingXPosition = startingCell.GetXPosition();
        int startingZPosition = startingCell.GetZPosition();
        GameObject room = new GameObject("Room");
        room.transform.parent = roomsObject.transform;
        rooms.Add(room);
        for (int x = startingXPosition; x < startingXPosition + width; x++) {
            for(int z = startingZPosition; z < startingZPosition + length; z++) {
                GameObject tile = grid.PlaceTileInCell(x, z);
                tile.transform.parent = room.transform;
            }
        } 
    }

    bool canPlaceRoom(GridCell cell, int width, int length) {
        // check if the proposed room is in bounds
        int xPosition = cell.GetXPosition();
        int zPosition = cell.GetZPosition();
        if (xPosition + width >= grid.GetTotalWidth() || zPosition + length >= grid.GetTotalLength()) {
            return false;
        }

        // check if all cells where the room would be are empty, and ensures a 1 cell gap between rooms.
        for(int i = xPosition - 1; i < xPosition + width + 1; i++) {
            for(int j = zPosition - 1; j < zPosition + length + 1; j++) {
                GridCell currentCell = grid.GetCell(i, j);
                if(currentCell == null) {
                    continue;
                }

                if(!currentCell.IsEmpty()) {
                    return false;
                }
            }
        }

        return true;
    }

}
