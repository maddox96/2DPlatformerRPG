using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{


    public class TickEffect : Effect
    {
        float tickInterval;
        public int numberOfTicks;
        int ticksLeft;
        [SerializeField]
        bool isTemporary;
        public float statsValue;
        public Effects.TickEffects effectType;

        protected override void Start()
        {
            base.Start();       
            tickInterval = duration / numberOfTicks;
            ticksLeft = numberOfTicks;
            StartCoroutine("ApplyAction");
        }

        void ApplyNonClassEffect(Effects.TickEffects effect)
        {
            switch (effect)
            {
                case Effects.TickEffects.Speed: ChangeStat(ref target.RunSpeed); break;
                case Effects.TickEffects.WeaponDamage: ChangeStat(ref target.WeaponDamage); isTemporary = true ; break;
                default: break;
            }
        }

        protected override void Action()
        {
            ApplyNonClassEffect(effectType);
        }

        protected override void OnDestroyEffect()
        {
            if (isTemporary) RestoreStats();
        }

        IEnumerator ExecAction()
        {
            while(ticksLeft > 0)
            {
                Action();
                ticksLeft--;
                yield return new WaitForSeconds(tickInterval);
            }
        }

        void ChangeStat(ref float stat)
        {
            stat += statsValue;
        }

        void ChangeStat(ref int stat)
        {
            stat += (int)statsValue;
        }

        void RestoreStats()
        {
            statsValue *= -1;
            statsValue *= numberOfTicks;
            Action();
        }

    }
}