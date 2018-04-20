using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{

    public class EffectData
    {
        public Effects.TickEffects effect;
        public float duration;
        public float statsValue;
        public int tickNumber;
        
        public EffectData(Effects.TickEffects _effect, float _duration, float _statsValue, int _tickNumber)
        {
            effect = _effect;
            duration = _duration;
            statsValue = _statsValue;
            tickNumber = _tickNumber;
        }
    }

    public abstract class Effect : MonoBehaviour {

        [HideInInspector]
        public Character target;
        [HideInInspector]
        public float duration;
    
        protected virtual void Start()
        {
            Destroy(gameObject, duration);
        }

        protected virtual void Action() { }
        protected virtual void OnDestroyEffect() { }
        void OnDestroy() { OnDestroyEffect(); }
    }
}