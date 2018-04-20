using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class BubbleShield : Spell
    {
        [SerializeField]
        GameObject healingSphere;


        public override string spellName
        {
            get
            {
                return "Bubble Shield";
            }
        }

        protected override Vector3 SetSpawnPosition()
        {
            Vector3 _temp = Utility.GetWallPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _temp.z = 0.0f;
            return _temp;
        }


        private void Start()
        {
            if (isUpgradeBought(0)) healingSphere.SetActive(true);
        }
    }
}