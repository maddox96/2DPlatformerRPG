using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{
    public class Blizzard : Spell
    {


        public override string spellName
        {
            get
            {
                return "Blizzard";
            }
        }

        protected override void Start()
        {
            base.Start();   
            //EffectManager.effectManager.ApplyTickEffect(caster, Effects.TickEffects.Speed, 25.0f, 350, 1); 
        }

    }
}