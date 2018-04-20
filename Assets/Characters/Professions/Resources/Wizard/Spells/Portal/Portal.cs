using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class Portal : Spell
    {
        static bool mainPortal = true;
        public static Portal lastSpawnedPortal;
        public Transform connectedPortal;

        public float speedDuration;
        float currentSpeedDuration;

        public override string spellName
        {
            get
            {
                return "Portal";
            }
        }

        protected override Vector3 SetSpawnPosition()
        {
            Vector3 _temp = Utility.GetWallPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), Utility.possiblePositions.GROUND);
            _temp.z = 0.0f;
            return _temp;
        }

        private void Update()
        {
            if (currentSpeedDuration >= 0.0f) currentSpeedDuration -= Time.deltaTime;
        }

        protected override void Start()
        {
            Profession p = new Wizard();
            //if (!upgrades.secondUpgrade) base.Start();
            if (mainPortal)
            {
                lastSpawnedPortal = this;
                mainPortal = false;
            }
            else
            {
                connectedPortal = lastSpawnedPortal.transform;
                lastSpawnedPortal.connectedPortal = transform;
                mainPortal = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(Input.GetKeyDown("w"))
            {
                if(collision.tag == "Player" && connectedPortal != null)
                {
                    collision.transform.position = connectedPortal.position;
                    if (isUpgradeBought(0) && currentSpeedDuration <= 0.0f)
                    {
                        EffectManager.effectManager.ApplyEffect(collision.GetComponent<Character>(), new EffectData(Effects.TickEffects.Speed, 3.0f, 250, 1));
                        currentSpeedDuration = speedDuration;
                    }
                }
            }
        }

    }
}