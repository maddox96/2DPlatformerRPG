using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{

    public class ProjectileSpikes : TimerTrap
    {

        Utility.Direction direction;
        List<Transform> fieryPoints;
        [SerializeField]
        LinearProjectile projectile;

        LinearProjectile projectileCopy;

        public override void timeEvent()
        {
            Action();
        }

        protected override void Start()
        {
            base.Start();
            projectileCopy = projectile;
            fieryPoints = Utility.GetChildrensWithTag(transform, "FieryPoint");
        }

        protected override bool EventStatement()
        {
            return true;
        }

        protected override void Action()
        {
            foreach (Transform t in fieryPoints)
                projectileCopy.Shoot(damage, t.position + (Vector3)Utility.DirectionToVector(direction) / 2.0f, direction);
        }

        protected override void OnBuild()
        {
            base.OnBuild();
            direction = Utility.PossiblePositionToDirection(facedPosition);
        }
    }
}