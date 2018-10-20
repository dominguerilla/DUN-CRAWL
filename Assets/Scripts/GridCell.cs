using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell {
    
    int x, z;
    GameObject tile;
    Grid parentGrid;
    Vector3 tileCenter;
    
    public GridCell(GameObject tilePrefab, Grid parentGrid, int xCoordinate, int zCoordinate) {
        this.x = xCoordinate;
        this.z = zCoordinate;

        this.tile = tilePrefab;
        this.parentGrid = parentGrid;
    }

    public bool IsEmpty() {
        return !tile;
    }

    public int GetXPosition() {
        return x;
    }

    public int GetZPosition() {
        return z;
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
        
        return this.tile;
    }

}
