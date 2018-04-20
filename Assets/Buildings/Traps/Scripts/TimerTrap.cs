using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public abstract class TimerTrap : Trap
    {

        bool _isReady;
        public bool isReady
        {
            set
            {
                _isReady = value;
                TryExecTimeEvent();
            }

            get
            {
                return _isReady;
            }
        }

        public float timeDuration;

        public abstract void timeEvent();
        protected abstract bool EventStatement();

        void TryExecTimeEvent()
        {
            if (EventStatement() && isReady)
            {
                timeEvent();
            }
        }

        void Ready()
        {
            isReady = true;
        }

       

        protected override void OnBuild()
        {
            base.OnBuild();
            InvokeRepeating("Ready", 0.0f, timeDuration);
        }
    }
}