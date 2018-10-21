using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingTreeGenerator : MonoBehaviour {

    public GameObject tilePrefab;
    public int gridWidth, gridLength;
    Grid grid;

	// Use this for initialization
	void Start () {
        if(!tilePrefab) {
            Debug.LogError("Tile prefab must be specified!");
            Destroy(this);
        }

        grid = new Grid(this.gameObject.transform.position, tilePrefab, gridWidth, gridLength);
	    GenerateCorridors();	
	}

    void GenerateCorridors() {
        Stack<ExploredCell> cellQueue = new Stack<ExploredCell>();
        int randomX = Random.Range(0, gridWidth - 1);
        int randomZ = Random.Range(0, gridLength - 1);

        ExploredCell firstCell = new ExploredCell(grid.GetCell(randomX, randomZ));
        cellQueue.Push(firstCell);
        firstCell.PlaceInCell(tilePrefab);

        while(cellQueue.Count > 0) {
        }
    }

}
