using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    int maxWidth;
    int maxLength;

    Vector3 lowerLeftCorner;
    GridCell[][] cells;
    GameObject tile;


    public Grid(Vector3 parentObject, GameObject tilePrefab, int width, int length) {

        this.lowerLeftCorner = parentObject;
        this.tile = tilePrefab;
        this.maxWidth = width;
        this.maxLength = length;

        // populate all cells with empty cell objects
        cells = new GridCell[maxWidth][];
        for(int i = 0; i < maxWidth; i++) {
            cells[i] = new GridCell[maxLength];
            for(int j = 0; j < maxLength; j++) {
                cells[i][j] = new GridCell(this, i, j);
            }
        }
    }
    
    public GameObject PlaceTileInCell(int x, int z) {

        GameObject placedObj = cells[x][z].PlaceInCell(tile);

        return placedObj;
    }

    public Vector3 GetLowerLeftCorner() {
        return lowerLeftCorner;
    }

    /// <summary>
    /// Returns a cell given its x and z coordinates, or null if the x/z is invalid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public GridCell GetCell(int x, int z) {
        //input validation
        if(x >= this.maxWidth || z >= this.maxLength || x < 0 || z < 0) {
            return null;
        }

        return this.cells[x][z];
    }

    public int GetTotalWidth() {
        return this.maxWidth;
    }

    public int GetTotalLength() {
        return this.maxLength;
    }
}
