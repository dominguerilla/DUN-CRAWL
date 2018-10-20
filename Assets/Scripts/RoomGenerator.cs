using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour {

    public GameObject tilePrefab;
    public int width, height = 100;

    public int numberOfRooms = 10;
    public int roomPlacementRetries = 50;

    public int minRoomWidth = 5;
    public int maxRoomWidth = 10;

    public int minRoomLength = 5;
    public int maxRoomLength = 10;

    Grid grid;

    private void Start() {
        if(maxRoomWidth >= width || maxRoomLength >= height) {
            Debug.LogError("Invalid maxRoomWidth/Length; must be less than width/height!");
            Destroy(this);
        }

		grid = new Grid(this.gameObject.transform.position, tilePrefab, width, height);
        GenerateRooms();
    }

    void GenerateRooms() {
        int numRooms = 0;
        for(int tries = 0; tries < roomPlacementRetries && numRooms < numberOfRooms; tries++) {

            int randomX = Random.Range(0, width - 1);
            int randomZ = Random.Range(0, height - 1);

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
        Debug.Log(string.Format("Creating {0} by {1} room at ({2},{3})", width, length, startingXPosition, startingZPosition));
        for(int x = startingXPosition; x < startingXPosition + width; x++) {
            for(int z = startingZPosition; z < startingZPosition + length; z++) {
                grid.PlaceTileInCell(x, z);
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

        // check if all cells where the room would be are empty
        for(int i = xPosition; i < width; i++) {
            for(int j = zPosition; j < length; j++) {
                if(!grid.GetCell(i, j).IsEmpty()) {
                    return false;
                }
            }
        }
        return true;
    }

}
