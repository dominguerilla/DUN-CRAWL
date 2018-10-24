using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrowingTreeGenerator : MonoBehaviour {

    public GameObject tilePrefab;
    public int gridWidth, gridLength;
    Grid grid;
    System.Random random;

	// Use this for initialization
	void Start () {
        if(!tilePrefab) {
            Debug.LogError("Tile prefab must be specified!");
            Destroy(this);
        }
        random = new System.Random();
        grid = new Grid(this.gameObject.transform.position, tilePrefab, gridWidth, gridLength);
	    StartCoroutine(GenerateCorridors());	
	}

    IEnumerator GenerateCorridors() {
        Stack<ExploredCell> cellStack = new Stack<ExploredCell>();
        int randomX = UnityEngine.Random.Range(0, gridWidth - 1);
        int randomZ = UnityEngine.Random.Range(0, gridLength - 1);

        ExploredCell firstCell = new ExploredCell(grid.GetCell(randomX, randomZ), this.random);
        //Debug.Log("Placing first corridor at " + randomX + ", " + randomZ);
        cellStack.Push(firstCell);
        firstCell.PlaceInCell(tilePrefab);

        while(cellStack.Count > 0) {
            ExploredCell currentCell = cellStack.Peek();
            GridCell neighbor = currentCell.GetCandidateNeighbor();
            if(neighbor == null){
                currentCell = cellStack.Pop();
                continue; 
            }else {
                // push neighbor onto stack
                ExploredCell newCell = new ExploredCell(neighbor, this.random);
                cellStack.Push(newCell);
                newCell.PlaceInCell(tilePrefab);
                yield return new WaitForEndOfFrame();
            }
        }

        Debug.Log("Finished generating corridors.");
    }
}
