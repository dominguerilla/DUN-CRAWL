using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour {

    class Room
    {
        List<GridCell> roomCells;
        GameObject roomObj;

        public Room(Grid grid, int startingXPosition, int startingZPosition, int width, int length)
        {
            this.roomCells = new List<GridCell>();
            roomObj = new GameObject("Room");

            for (int x = startingXPosition; x < startingXPosition + width; x++)
            {
                for (int z = startingZPosition; z < startingZPosition + length; z++)
                {
                    GridCell roomCell = grid.PlaceTileInCell(x, z);
                    AddCell(roomCell);
                    GameObject tile = roomCell.GetTile();
                    tile.transform.parent = roomObj.transform;
                }
            }
        }

        public void AddCell(GridCell cell)
        {
            this.roomCells.Add(cell);
        }

        public GameObject GetGameObject()
        {
            return this.roomObj;
        }

        public void Reset()
        {
            foreach (GridCell cell in roomCells)
            {
                cell.ClearCell();
            }
            GameObject.Destroy(roomObj);
            roomObj = null;
        }
    }

    public GameObject tilePrefab;
    public int gridWidth, gridLength = 100;

    public int numberOfRooms = 10;
    public int roomPlacementRetries = 50;

    public int minRoomWidth = 5;
    public int maxRoomWidth = 10;

    public int minRoomLength = 5;
    public int maxRoomLength = 10;

    List<Room> rooms;
    GameObject roomObjectList;
    
    public void GenerateRooms(Grid grid) {

        if (maxRoomWidth >= gridWidth || maxRoomLength >= gridLength) {
            Debug.LogError("Invalid maxRoomWidth/Length; must be less than width/height!");
            return;
        }

        rooms = new List<Room>();
        roomObjectList = new GameObject("Rooms");
        int numRooms = 0;
        for(int tries = 0; tries < roomPlacementRetries && numRooms < numberOfRooms; tries++) {

            int randomX = Random.Range(0, gridWidth - 1);
            int randomZ = Random.Range(0, gridLength - 1);

            int randomWidth = Random.Range(minRoomWidth, maxRoomWidth);
            int randomHeight = Random.Range(minRoomLength, maxRoomLength);

            GridCell currentCell = grid.GetCell(randomX, randomZ);
            if(canPlaceRoom(grid, currentCell, randomWidth, randomHeight)){
                PlaceRoom(grid, currentCell, randomWidth, randomHeight);
                numRooms++;
            }else {
                continue;
            }
        }
        Debug.Log("Finished placing " + numRooms + " rooms.");
    }

    public void DestroyRooms()
    {
        if (rooms != null)
        {
            foreach (Room room in rooms)
            {
                room.Reset();
            }
        }

        rooms = null;
        Destroy(roomObjectList);
        roomObjectList = null;
    }

    void PlaceRoom(Grid grid, GridCell startingCell, int width, int length) {
        int startingXPosition = startingCell.GetXPosition();
        int startingZPosition = startingCell.GetZPosition();

        Room room = new Room(grid, startingXPosition, startingZPosition, width, length);
        rooms.Add(room);
        GameObject roomObj = room.GetGameObject();
        roomObj.transform.parent = roomObjectList.transform;
    }

    bool canPlaceRoom(Grid grid, GridCell cell, int width, int length) {
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
