using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    int x;
    int z;

    Vector3 lowerLeftCorner;
    GridCell[][] cells;
    GameObject tile;

    public Grid(Vector3 parentObject, GameObject tilePrefab, int width, int depth) {

        this.lowerLeftCorner = parentObject;
        this.tile = tilePrefab;
        this.x = width;
        this.z = depth;

        // populate all cells with empty cell objects
        cells = new GridCell[x][];
        for(int i = 0; i < x; i++) {
            cells[i] = new GridCell[z];
            for(int j = 0; j < z; j++) {
                cells[i][j] = new GridCell(tile, this, i, j);
            }
        }
    }

    public GameObject PlaceInCell(GameObject prefab, int x, int z) {

        GameObject placedObj = cells[x][z].PlaceInCell(prefab);

        return placedObj;
    }

    public Vector3 GetLowerLeftCorner() {
        return lowerLeftCorner;
    }
}
