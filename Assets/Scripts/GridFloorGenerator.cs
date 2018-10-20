using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFloorGenerator : MonoBehaviour {
    
    public GameObject tilePrefab;
    public int width, height = 1;

    Grid grid;

	// Use this for initialization
	void Start () {
		grid = new Grid(this.gameObject.transform.position, tilePrefab, width, height);
	    grid.PlaceInCell(tilePrefab, 0, 0);	
	    grid.PlaceInCell(tilePrefab, 0, 1);	
	    grid.PlaceInCell(tilePrefab, 1, 0);	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
