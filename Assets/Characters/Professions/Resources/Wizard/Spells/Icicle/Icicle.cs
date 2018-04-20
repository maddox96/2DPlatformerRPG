using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{

    public class Icicle : Spell
    {
        public override string spellName
        {
            get
            {
                return "Icicle";
            }
        }

        int fragility = 3;
        protected override void Start()
        {
            base.Start();
            if (isUpgradeBought(0)) fragility *= 3;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character _temp = collision.GetComponent<Character>();
            if (_temp)
            {
                fragility--;
                _temp.ApplyDamage(spellDMG);
                if(isUpgradeBought(0)) EffectManager.effectManager.ApplyEffect(_temp, new EffectData(Effects.TickEffects.Speed, 3.0f, 250, 1));
                if (fragility < 1) Destroy(gameObject);
            }
        }
    }
}