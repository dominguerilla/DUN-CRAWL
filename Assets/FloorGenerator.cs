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

        StartCoroutine(GenerateFloor());
	}

    IEnumerator GenerateFloor() {
        GameObject firstFloor = GameObject.Instantiate<GameObject>(floor);
        BoxCollider firstBox = firstFloor.GetComponent<BoxCollider>();

        Vector3 topLeftCornerOfFloor = anchor.transform.TransformPoint(firstBox.center - new Vector3(-firstBox.size.x / 2, 0, firstBox.size.z / 2));
        firstFloor.transform.position = topLeftCornerOfFloor; 

        yield return new WaitForSeconds(0.1f);

        for(int z = 0; z < length; z++) {
            for(int x = 0; x < width; x++) {
                //skip the first floor piece, since we already made it
                if(x == 0 && z == 0)
                    continue;
                GameObject currentFloor = GameObject.Instantiate<GameObject>(floor);
                currentFloor.transform.position = firstFloor.transform.position + new Vector3(firstBox.size.x * x, 0, -firstBox.size.z * z);
            }
        }
    }
	
}
