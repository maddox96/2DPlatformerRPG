using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class GameManager : MonoBehaviour {

        public static GameManager manager;
        [HideInInspector]
        public Character player;

        private void Awake()
        {
            manager = this;
            player = Utility.GetPlayer();
        } 
    }
}