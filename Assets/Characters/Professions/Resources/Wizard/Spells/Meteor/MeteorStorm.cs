using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{

    public class MeteorStorm : Spell
    {


        public override string spellName
        {
            get
            {
                return "Meteor storm";
            }
        }

        public VerticalProjectile meteor;
        
        void SpawnMeteor()
        {
         
           meteor.Shoot((int)spellDMG, new Vector3(Random.Range(-5.0f, 5.0f), 15.0f, 0.0f));
            
        }

        protected override void Start()
        {
            base.Start();
            InvokeRepeating("SpawnMeteor", 3.0f, 0.1f);
        }
    }

}