using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{
    public class TrapChildrenOTE : MonoBehaviour
    {


        Trap trap;

        private void Start()
        {
            trap = GetComponentInParent<Trap>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (trap.isPreview) return;

            Character c = collision.GetComponent<Character>();
            if (c != null)
                trap.PerformAction(c);
            else trap.PerformAction();
        }
    }
}