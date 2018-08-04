using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    bool isColliding = false;

    public bool isCollidingWithOther(){
        return isColliding;
    }

}
