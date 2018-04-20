using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatedPixelPack
{
    public class EffectManager : MonoBehaviour
    {
        /// <summary>
        /// EffectManager can put on target buffs/ debuffs. Also storage effect prefabs. 
        /// </summary>

        #region SINGLETON
        public static EffectManager effectManager;

        private void Start()
        {
            effectManager = this;
        }
        #endregion

        #region PREFABS
        public TickEffect BurnEffectPrefab;
        public TickEffect SpeedEffectPrefab;
        public TickEffect tickEffectPrefab;
        public BoolEffect BoolEffectPrefab;

        TickEffect GetTickEffectPrefab(Effects.TickEffects effect)
        {
            switch(effect)
            {
                case Effects.TickEffects.Speed: return SpeedEffectPrefab;
                case Effects.TickEffects.Poison: return BurnEffectPrefab;
                default: return tickEffectPrefab;
            }
        }

        #endregion
   
        #region SPAWN METHODS

        public void ApplyCrowdControl(Character target, Effects.BoolEffects cc, float duration)
        {
            BoolEffect tempEffect = Instantiate(BoolEffectPrefab, target.transform);
            tempEffect.target = target;
            tempEffect.duration = duration;
            tempEffect.cc = cc;
        }
   
        public void ApplyTickEffect(Character target, Effects.TickEffects effect, float duration, float actionValue, int tickNumbers = 1)
        {
            TickEffect tempEffect = Instantiate(GetTickEffectPrefab(effect), target.transform, false);
            tempEffect.duration = duration;
            tempEffect.statsValue = actionValue;
            tempEffect.target = target;
            tempEffect.numberOfTicks = tickNumbers;
            tempEffect.effectType = effect;
        }

        #endregion
    }
}