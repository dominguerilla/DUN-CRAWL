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
    GameObject corridors;
    bool isGenerating = false;

    public void StartCorridorGeneration(Grid grid){
        if (!isGenerating)
        {
            this.grid = grid;
            corridors = new GameObject("Corridors");

            if (!tilePrefab)
            {
                Debug.LogError("Tile prefab must be specified!");
                Destroy(this);
            }
            random = new System.Random();
            StartCoroutine(GenerateCorridors());
        }
        else
        {
            Debug.LogError("Tree generator is still running!");
        }
    }

    public void ResetGenerator()
    {
        this.grid = null;
        this.random = null;
        Destroy(corridors);
        corridors = null;
    }

    IEnumerator GenerateCorridors() {
        isGenerating = true;
        Stack<ExploredCell> cellStack = new Stack<ExploredCell>();
        // we don't start trees at the edges of grid
        for(int X = 1; X < gridWidth - 1; X++){
            for(int Z = 1; Z < gridLength - 1; Z++){
                ExploredCell rootECell = new ExploredCell(grid.GetCell(X, Z), this.random);

                // check if this cell is a valid one to start
                if (!isValidRootCell(rootECell))
                {
                    continue;
                }

                GameObject corridor = new GameObject("Corridor");
                corridor.transform.parent = corridors.transform;

                cellStack.Push(rootECell);
                GameObject corridorRoot = rootECell.PlaceInCell(tilePrefab);

                corridorRoot.transform.parent = corridor.transform;


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
                        GameObject branchCell = newCell.PlaceInCell(tilePrefab);
                        branchCell.transform.parent = corridor.transform;
                        yield return new WaitForEndOfFrame();
                    }
                }

            }
        }

        TrimTree();

        RemoveEmptyCorridorObjects();

        isGenerating = false;
        Debug.Log("Finished generating corridors.");
    }

    IEnumerator RemoveEmptyCorridorObjects()
    {
        bool didDelete;
        do
        {
            didDelete = false;
            foreach (Transform corridor in corridors.transform)
            {
                if (corridor.childCount <= 1)
                {
                    Destroy(corridor.gameObject);
                    didDelete = true;
                }
                yield return null;
            }

        } while (didDelete);
    }

    bool isValidRootCell(ExploredCell rootECell)
    {
        GridCell cell = rootECell.GetCell();

        if (!cell.IsEmpty())
        {
            return false;
        }
        List<GridCell> emptyNeighbors = rootECell.GetEmptyNeighbors(cell);
        if (emptyNeighbors.Count < 4)
        {
            return false;
        }
        return true;
    }

    void TrimTree(){
        // initial trimming
        for (int i = 0; i < trimLength; i++){
            for(int x = 0; x < grid.GetTotalLength(); x++){
                for(int z = 0; z < grid.GetTotalWidth(); z++){
                    TrimCell(x,z);
                }
            }
        }

        // TODO this doesn't work fully yet. I still see loner cells.
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
            cell.ClearCell();
            return true;
        }
        return false;
    }

    public bool isStillGenerating()
    {
        return isGenerating;
    }
}
