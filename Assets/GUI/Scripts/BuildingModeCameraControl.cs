using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingModeCameraControl : MonoBehaviour {

    [SerializeField]
    float speed = 5.0f;
	
	void Update ()
    {
        transform.Translate(new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed));
	}
}
