using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExploredCell {
    
    GridCell cell;
    List<GridCell> unexploredNeighbors;
    System.Random random;

    public ExploredCell(GridCell cell, System.Random random = null) {
        this.cell = cell;
        if(random == null){
            random = new System.Random();
        }else{
            this.random = random;
        }
        unexploredNeighbors = new List<GridCell>();

        int[] directions = { 0, 1, 2, 3 };
        foreach(int direction in directions) {
            GridCell neighbor = cell.GetNeighbor((GridCell.Neighbor)direction); 
            if(neighbor != null && neighbor.IsEmpty()) {
                unexploredNeighbors.Add(neighbor);
            }
        }
    }

    public GameObject PlaceInCell(GameObject prefab) {
        return cell.PlaceInCell(prefab);
    }

    /// <summary>
    /// Returns a random unexplored empty neighbor, or null if there are none available.
    /// </summary>
    /// <returns></returns>
    public GridCell GetRandomEmptyNeighbor() {
        GridCell neighbor = null;
        while (unexploredNeighbors.Count > 0){
            neighbor = unexploredNeighbors[random.Next(unexploredNeighbors.Count)];
            if(neighbor.IsEmpty()){
                break;
            }else{
                unexploredNeighbors.Remove(neighbor);
            }
        }
        return neighbor;
    }

    public GridCell GetCell(){
        return cell;
    }

}
