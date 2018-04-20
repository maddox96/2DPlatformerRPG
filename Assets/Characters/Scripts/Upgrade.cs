using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    [System.Serializable]
    public class Upgrade : IBuyable {

        public int level, maxLevel;
        public string name, description;
        [SerializeField]
        private int _cost;
        public int cost
        {
            get
            {
                return _cost;
            }
        }

        public bool isBought { get { return level > 0; } }

        public void Buy()
        {
            if(level < maxLevel)
            {
                GameManager.manager.player.spendMoney(this);
                level++;
            }      
        }

    }
}