using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionConnector {

    Grid grid;

    public RegionConnector(Grid grid) {
        this.grid = grid;
    }

    public void SetGrid(Grid grid) {
        this.grid = grid;
    }

    public void ConnectRegions() {
        // first, we're getting a list of GROUPS of connecting region
        //
        // for each empty cell in grid
        // check to see if placing a grid there connects two separate regions
        // if it does, check if placing one down lets it have two empty neighbors
        // if it does, check if it's touching a cell that's already designated as a possible connector for the two regions
        // if it's touching, put it into that group of connectors
        // if it's not, create a new group of connectors that includes this cell
        //
        // second, we iterate over each group of connectors and pick one cell randomly to be a real connector

    }

    public List<GridCell> GetPossibleConnectors() {
        return null;
    }

}
