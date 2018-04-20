using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class EffectManager : MonoBehaviour
    {
        /// <summary>
        /// EffectManager can put on target buffs/ debuffs. Also storage effect prefabs. 
        /// </summary>

        #region SINGLETON
        public static EffectManager effectManager;

        private void Awake()
        {
            effectManager = this;
        }
        #endregion

        #region PREFABS

        public TickEffect[] loadedEffects;
        public TickEffect tickEffectPrefab;
        public BoolEffect BoolEffectPrefab;
  
        TickEffect GetTickEffectPrefab(Effects.TickEffects effect)
        {
            foreach(TickEffect e in loadedEffects)
            {
                if (e.effectType == effect)
                    return e;
            }

            return tickEffectPrefab;
        }

        private void Start()
        {
            string path = "Effects";
            loadedEffects = Resources.LoadAll<TickEffect>(path);
        }

        #endregion

        #region SPAWN METHODS


        public void ApplyEffect(Character target, Effects.BoolEffects cc, float duration)
        {
            BoolEffect tempEffect = Instantiate(BoolEffectPrefab, target.transform);
            tempEffect.target = target;
            tempEffect.duration = duration;
            tempEffect.cc = cc;
        }
   
        public void ApplyEffect(Character target, EffectData effectData)
        {
            TickEffect tempEffect = Instantiate(GetTickEffectPrefab(effectData.effect), target.transform, false);
            tempEffect.duration = effectData.duration;
            tempEffect.statsValue = effectData.statsValue;
            tempEffect.target = target;
            tempEffect.numberOfTicks = effectData.tickNumber;
            tempEffect.effectType = effectData.effect;
        }

        #endregion
    }
}