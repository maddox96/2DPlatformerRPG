﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimatedPixelPack
{

    public class Icicle : Spell
    {

        public override Spell Cast(Spell objToCast, Character caster)
        {
            objToCast.GetComponent<Projectile>().Shoot(objToCast, caster);
            return null;
        }
    }
}