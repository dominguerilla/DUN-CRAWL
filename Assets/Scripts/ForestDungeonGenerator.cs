using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GrowingTreeGenerator))]
[RequireComponent(typeof(RoomGenerator))]
public class ForestDungeonGenerator : MonoBehaviour {
    public GameObject tilePrefab;

    [Header("Grid settings")]
    public int gridWidth = 30;
    public int gridLength = 30;

    [Header("Room settings")]
    public int numberOfRooms = 10;
    public int roomPlacementRetries = 50;

    public int minRoomWidth = 5;
    public int maxRoomWidth = 10;

    public int minRoomLength = 5;
    public int maxRoomLength = 10;

    [Header("Corridor settings")]
    public int trimLength = 1;

    Grid grid;
    GrowingTreeGenerator treeGen;
    RoomGenerator roomGen;

	// Use this for initialization
	void Start () {

        this.grid = new Grid(this.transform.position, tilePrefab, gridWidth, gridLength);
	    treeGen = GetComponent<GrowingTreeGenerator>();
        roomGen = GetComponent<RoomGenerator>();

	}

    private void Update()
    {
        if (!treeGen.isStillGenerating())
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GenerateDungeon();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                EraseDungeon();
            }
        }
    }

    private void InitGenerator()
    {
        treeGen.tilePrefab = this.tilePrefab;
        roomGen.tilePrefab = this.tilePrefab;

        treeGen.gridWidth = this.gridWidth;
        treeGen.gridLength = this.gridLength;
        roomGen.gridWidth = this.gridWidth;
        roomGen.gridLength = this.gridLength;

        roomGen.numberOfRooms = this.numberOfRooms;
        roomGen.roomPlacementRetries = this.roomPlacementRetries;
        roomGen.minRoomLength = this.minRoomLength;
        roomGen.maxRoomLength = this.maxRoomLength;
        roomGen.minRoomWidth = this.minRoomWidth;
        roomGen.maxRoomLength = this.maxRoomLength;

        treeGen.trimLength = this.trimLength;
    }

    public void EraseDungeon()
    {
        roomGen.DestroyRooms();
        treeGen.ResetGenerator();
        grid.ResetGrid();
    }

    public void GenerateDungeon()
    {
        EraseDungeon();

        InitGenerator();

        roomGen.GenerateRooms(this.grid);
        treeGen.StartCorridorGeneration(this.grid);
    }

}
