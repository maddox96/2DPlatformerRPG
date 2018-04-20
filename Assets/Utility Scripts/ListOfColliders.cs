using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfColliders : MonoBehaviour {

    public List<Transform> colliders;

    public bool isColliding
    {
        get
        {
            return colliders.Count == 0 ? false : true;
        }
    }

    private void Start()
    {
        colliders = new List<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision.transform);
    }

}
