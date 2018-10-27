using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExploredCell {
    
    GridCell cell;
    System.Random random;

    public ExploredCell(GridCell cell, System.Random random = null) {
        this.cell = cell;
        if(random == null){
            random = new System.Random();
        }else{
            this.random = random;
        }

    }

    public GameObject PlaceInCell(GameObject prefab) {
        return cell.PlaceInCell(prefab);
    }
    

    /// <summary>
    /// Returns a random empty neighbor with three empty neighbors, or null if there are none available.
    /// </summary>
    /// <returns></returns>
    public GridCell GetCandidateNeighbor() {
        GridCell neighbor = null;
        List<GridCell> candidateNeighbors = GetEmptyNeighbors(this.cell);
        while(candidateNeighbors.Count > 0 && neighbor == null){
            // check if random neighbor has at least three empty neighbors
            GridCell tempNeighbor = candidateNeighbors[random.Next(candidateNeighbors.Count)];
            List<GridCell> tempEmptyNeighbors = GetEmptyNeighbors(tempNeighbor);
            if(tempEmptyNeighbors.Count >= 3){
                neighbor = tempNeighbor;
                break;
            }else{
                candidateNeighbors.Remove(tempNeighbor);
            }
        }
        return neighbor;
    }

    // should probably be a static function or not in this class
    public List<GridCell> GetEmptyNeighbors(GridCell targetCell){
        List<GridCell> emptyNeighbors = new List<GridCell>();
        int[] directions = { 0, 1, 2, 3 };
        foreach (int direction in directions) {
            GridCell otherNeighbor = targetCell.GetNeighbor((GridCell.Neighbor)direction); 
            if(otherNeighbor != null && otherNeighbor.IsEmpty()) {
                emptyNeighbors.Add(otherNeighbor);
            }
        }
        return emptyNeighbors;
    }

    public GridCell GetCell(){
        return cell;
    }

}
