using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {
    
    /// <summary>
    /// The relative position where the floor will be generated from. 
    /// The top left corner of the floor piece will be made on this point.
    /// </summary>
    public Transform anchor;

    public int width = 10;
    public int length = 10;

    /// <summary>
    /// Should have a box collider.
    /// </summary>
    public GameObject floor;

	// Use this for initialization
	void Start () {
	    if(!anchor) {
            Debug.LogError("No anchor set!");
            Destroy(this);
        }

        GenerateFloor();
	}

    void GenerateFloor() {
        GameObject firstFloor = GameObject.Instantiate<GameObject>(floor);
        BoxCollider box = firstFloor.GetComponent<BoxCollider>();

        Vector3 topLeftCornerOfFloor = anchor.transform.TransformPoint(box.center - new Vector3(-box.size.x / 2, 0, box.size.z / 2));
        firstFloor.transform.position = topLeftCornerOfFloor; 
        
    }
	
}
