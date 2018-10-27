using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrowingTreeGenerator : MonoBehaviour {

    public GameObject tilePrefab;
    public int gridWidth, gridLength;
    public int trimLength;

    Grid grid;
    System.Random random;

    public void SetGrid(Grid grid){
        this.grid = grid;
    }
    
    public void StartCorridorGeneration(Grid grid){
        this.grid = grid;

        if(!tilePrefab) {
            Debug.LogError("Tile prefab must be specified!");
            Destroy(this);
        }
        random = new System.Random();
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
        StartCoroutine(TrimTree());
    }

    IEnumerator TrimTree(){
        Debug.Log("Trimming tree...");
        for(int i = 0; i < trimLength; i++){
            for(int x = 0; x < grid.GetTotalLength(); x++){
                for(int z = 0; z < grid.GetTotalWidth(); z++){
                    TrimCell(x,z);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        Debug.Log("Finished trimming tree.");
    }

    void TrimCell(int x, int z){
        GridCell cell = grid.GetCell(x, z);
        int[] directions = { 0, 1, 2, 3 };

        int numberOfNeighbors = 0;
        foreach (int direction in directions){
            GridCell neighbor = cell.GetNeighbor((GridCell.Neighbor)direction);
            if(neighbor == null || neighbor.IsEmpty()){
                numberOfNeighbors++;
            }
        }

        if(numberOfNeighbors >=3){
            GameObject floorObj = cell.GetTile();
            Destroy(floorObj);
            cell.ClearCellFlag();
        }
    }

    
}
