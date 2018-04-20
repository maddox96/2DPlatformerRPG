using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimatedPixelPack
{
    public class Blizzard : Spell
    {

        public override Spell Cast(Spell instance, Character caster)
        {
            base.Cast(instance, caster).transform.SetParent(Camera.main.transform, false);
            return null;
        }

        protected override void Start()
        {
            base.Start();   
            EffectManager.effectManager.ApplyTickEffect(caster, Effects.TickEffects.Speed, 25.0f, 350, 1); 
        }

    }
}