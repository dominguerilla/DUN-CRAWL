using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploredCell {
    
    GridCell cell;
    Stack<GridCell> unexploredNeighbors;

    public ExploredCell(GridCell cell) {
        this.cell = cell;
        unexploredNeighbors = new Stack<GridCell>();

        int[] directions = { 0, 1, 2, 3 };
        foreach(int direction in directions) {
            GridCell neighbor = cell.GetNeighbor((GridCell.Neighbor)direction); 
            if(neighbor != null && neighbor.IsEmpty()) {
                unexploredNeighbors.Push(neighbor);
            }
        }
    }

    public GameObject PlaceInCell(GameObject prefab) {
        return cell.PlaceInCell(prefab);
    }

    public GridCell GetRandomNeighbor() {
        GridCell.Neighbor neighbor = (GridCell.Neighbor)Random.Range(0, 3);
        return cell.GetNeighbor(neighbor);
    }

}
