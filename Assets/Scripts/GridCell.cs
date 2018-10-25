using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell {
    
    public enum Neighbor {
        UPPER,
        LOWER,
        LEFT,
        RIGHT
    }

    int x, z;
    GameObject tile;
    Grid parentGrid;
    Vector3 tileCenter;
    bool placedInCell = false;
    
    public GridCell(Grid parentGrid, int xCoordinate, int zCoordinate) {
        this.x = xCoordinate;
        this.z = zCoordinate;

        this.parentGrid = parentGrid;
    }

    public bool IsEmpty() {
        return !placedInCell;
    }

    public int GetXPosition() {
        return x;
    }

    public int GetZPosition() {
        return z;
    }

    public GameObject GetTile(){
        return this.tile;
    }

    public void ClearCellFlag(){
        if(!this.placedInCell){
            this.placedInCell = false;
            this.tile = null;
        }
    }

    /// <summary>
    /// Note that the prefab should have a box collider. All cells of the same grid should have the same tile prefab.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject PlaceInCell(GameObject prefab) {

        this.tile = GameObject.Instantiate<GameObject>(prefab);
        BoxCollider box = this.tile.GetComponent<BoxCollider>();

        Vector3 gridDLeftCorner = parentGrid.GetLowerLeftCorner();
        this.tileCenter = gridDLeftCorner + new Vector3((box.size.x) * this.x, gridDLeftCorner.y, (box.size.z) * this.z);
        
        this.tile.transform.position = this.tileCenter;
        placedInCell = true;
        return this.tile;
    }

    public GridCell GetNeighbor(Neighbor neighbor) {
        switch(neighbor) {
            case Neighbor.UPPER:
                return parentGrid.GetCell(this.x, this.z + 1);
            case Neighbor.LOWER:
                return parentGrid.GetCell(this.x, this.z - 1);
            case Neighbor.LEFT:
                return parentGrid.GetCell(this.x - 1, this.z);
            case Neighbor.RIGHT:
                return parentGrid.GetCell(this.x + 1, this.z);
            default:
                return null;
        }
    }
}
