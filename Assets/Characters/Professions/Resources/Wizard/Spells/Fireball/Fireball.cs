using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class Fireball : Spell
    {
        /*private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 3.0f);
        }*/


        public override string spellName
        {
            get
            {
                return "Fireball";
            }
        }

        public GameObject burningTail;

        public override bool castAble
        {
            get
            {
                return isUpgradeBought(0);
            }
        }
  
        protected override void Start()
        {
            base.Start();
            if (isUpgradeBought(2)) burningTail.SetActive(true);

            if (castAble)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + castedTime);
                TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();
                foreach(TrailRenderer trail in trails)
                {
                    trail.widthMultiplier *= castedTime + 1;
                    trail.time *= castedTime + 1;
                }
                transform.localScale += new Vector3(castedTime, castedTime);
                // dmg to do
            }
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {           
            List<Character> c = AreaEffect.GetCharacters(null, transform.position, 3.0f);
            if (isUpgradeBought(1)) AreaEffect.ApplyAreaEffect(c, new EffectData(Effects.TickEffects.Burn, 5.0f, 150, 5));
        }
    }
}