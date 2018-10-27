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
        // we don't start trees at the edges of grid
        for(int X = 1; X < gridWidth - 1; X++){
            for(int Z = 1; Z < gridLength - 1; Z++){
                ExploredCell rootECell = new ExploredCell(grid.GetCell(X, Z), this.random);

                // check if this cell is a valid one to start
                GridCell rootCell = rootECell.GetCell();
                if(!rootCell.IsEmpty()){
                    continue;
                }
                List<GridCell> emptyNeighbors = rootECell.GetEmptyNeighbors(rootCell);
                if(emptyNeighbors.Count < 4){
                    continue;
                }

                cellStack.Push(rootECell);
                rootECell.PlaceInCell(tilePrefab);

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

            }
        }


        Debug.Log("Finished generating corridors.");
        TrimTree();
    }

    void TrimTree(){
        Debug.Log("Trimming tree...");
        // initial trimming
        for (int i = 0; i < trimLength; i++){
            for(int x = 0; x < grid.GetTotalLength(); x++){
                for(int z = 0; z < grid.GetTotalWidth(); z++){
                    TrimCell(x,z);
                }
            }
        }

        // removing 'loner' cells
        for(int x = 0; x < grid.GetTotalLength(); x++){
            for(int z = 0; z < grid.GetTotalWidth(); z++){
                TrimCell(x,z,4);
            }
        }
        Debug.Log("Finished trimming tree.");
    }

    // returns true if the cell was trimmed, false otherwise
    bool TrimCell(int x, int z, int threshold = 3){
        GridCell cell = grid.GetCell(x, z);
        int[] directions = { 0, 1, 2, 3 };

        int numberOfEmptyNeighbors = 0;
        foreach (int direction in directions){
            GridCell neighbor = cell.GetNeighbor((GridCell.Neighbor)direction);
            if(neighbor == null || neighbor.IsEmpty()){
                numberOfEmptyNeighbors++;
            }
        }

        if(numberOfEmptyNeighbors >= threshold){
            GameObject floorObj = cell.GetTile();
            Destroy(floorObj);
            cell.ClearCellFlag();
            return true;
        }
        return false;
    }

    
}
