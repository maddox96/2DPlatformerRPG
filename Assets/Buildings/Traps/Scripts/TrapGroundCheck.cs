using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGroundCheck : MonoBehaviour {


    BoxCollider2D[] groundCheckers;

	void Start ()
    {
        groundCheckers = new BoxCollider2D[2];
        groundCheckers = GetComponentsInChildren<BoxCollider2D>();	
	}
}
