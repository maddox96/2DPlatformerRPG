using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatedPixelPack
{
    public abstract class Spell : MonoBehaviour
    {
        public Sprite icon;
        public float lifeTime = 10.0f;
        public int manaCost, spellDMG;
        public float  castDuration, cooldown = 5.0f;
        protected Vector3 target { get; private set; }
        [HideInInspector]
        public Character caster;

        public virtual Spell Cast(Spell instance, Character caster)
        {
            Spell spell = Instantiate(instance);
            spell.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spell.caster = caster;
            return spell;
        }

        protected virtual void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        protected virtual void Update()
        {

        }
    }
}