using UnityEngine;

namespace Portfolio
{
    public class BoolEffect : Effect
    {
        [HideInInspector]
        public Effects.BoolEffects cc;

        private void Start()
        {
            Action();
            Destroy(gameObject, duration);
        }

        protected override void Action()
        {
            SetCC(true);
        }

        protected override void OnDestroyEffect()
        {
            SetCC(false);
        }

        void SetCC(bool state)
        {
            if(target != null)
                target.currentCC[cc] = state;
        }


    }
}