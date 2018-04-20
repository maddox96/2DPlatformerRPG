using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatedPixelPack
{

    public class MeteorStorm : Spell
    {

        public VerticalProjectile meteor;
        
        void SpawnMeteor()
        {
            meteor.Shoot(meteor, spellDMG, new Vector3(Random.Range(-5.0f, 5.0f), 15.0f, 0.0f));
        }

        protected override void Start()
        {
            base.Start();
            InvokeRepeating("SpawnMeteor", 3.0f, 0.1f);
        }
    }

}