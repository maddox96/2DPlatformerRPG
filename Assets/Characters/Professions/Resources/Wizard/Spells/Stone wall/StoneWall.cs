using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{
    public class StoneWall : Spell
    {
        public float direction;
        Rigidbody2D rb;
        BoxCollider2D[] colliders;

        public override string spellName
        {
            get
            {
                return "Ground wall";
            }
        }

        protected override Vector3 SetSpawnPosition()
        {
            return Utility.GetWallPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        protected override void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            colliders = GetComponents<BoxCollider2D>();

            if (isUpgradeBought(0))
            {
                foreach(BoxCollider2D c in colliders)
                {
                    if (c.isTrigger) c.enabled = true;
                }
            }
                  
            // if (upgrades.secondUpgrade) GetComponentInChildren<EdgeCollider2D>().enabled = true;
            if (caster) direction = caster.CurrentDirection == Character.Direction.Left ? -1 : 1;
            if(direction == -1)
            {
                transform.Rotate(new Vector3(0.0f, 0.0f, 90f));
            }

            base.Start();

        }
    }
}