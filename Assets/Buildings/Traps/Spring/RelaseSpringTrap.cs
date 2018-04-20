using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{

    public class RelaseSpringTrap : MonoBehaviour {

        SpringTrap trap;

        private void Start()
        {
            trap = GetComponentInParent<SpringTrap>();
        }


    }
}