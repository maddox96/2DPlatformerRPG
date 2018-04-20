using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class Mushroom : Trap
    {

        protected override void Action()
        {
            List<Character> l = AreaEffect.GetCharacters(null, transform.position, 2.0f);
            AreaEffect.ApplyAreaEffect(l, new EffectData(Effects.TickEffects.Poison, 13.0f, 5.0f, 5));
        }

    }
}