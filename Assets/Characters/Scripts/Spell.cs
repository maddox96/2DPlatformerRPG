using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio
{
    public abstract class Spell : MonoBehaviour, IIconBarGeneratable
    {

        public abstract string spellName { get; }
        public Sprite spellBookIcon;

        [SerializeField]
        private Upgrade[] upgrades;
       
        public Upgrade[] GetSpellUpgrades()
        {
            return upgrades;
        }

        public bool isUpgradeBought(int index)
        {
            if (index >= upgrades.Length)
            {
                Debug.LogError("There is no upgrade with " + index + " index.");
                return false;
            }

            return upgrades[index].isBought;
        }

        public float maxCastValue = 4.0f;

        private float _castedTime;
        public float castedTime
        {
            set { _castedTime = value; }
            get
            {
                if (castAble)
                    return Mathf.Clamp(_castedTime, 0.0f, 4.0f);
                else
                {
                    Debug.Log("You are trying to get CastTime on nonCastable spell");
                    return 0;
                }
            }
        }

        [SerializeField]
        protected bool _castAble;
        public virtual bool castAble
        {
            get
            {
                return _castAble;
            }
        }
        public int spellDMG;
        public float lifeTime = 10.0f;
        public int manaCost;
        public float cooldown = 1.0f;
        protected Vector3 target { get; private set; }
        public Character caster;

        public Spell Cast(Character caster)
        {
            this.caster = caster;
            target = SetTarget();
            Spell spell = Instantiate(this, SetSpawnPosition(), SetSpawnRotation());
            Projectile isProjectile = spell.GetComponent<Projectile>();
            if(isProjectile != null)
            {
                isProjectile.Owner = caster;
                isProjectile.Damage = (int)spellDMG;
            }

            return spell;
        }

        /// <summary>
        /// Spell target. Default returns mouse position;
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 SetTarget()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public virtual Spell Cast(Vector3 target, Character caster = null)
        {
            Projectile isProjectile = GetComponent<Projectile>();
            if (isProjectile != null)
            {
                isProjectile = isProjectile.Shoot(spellDMG, target);
                isProjectile.Owner = caster;
                return this;
            }

            Spell spell = Instantiate(this, target, Quaternion.identity);
            spell.caster = caster;
            return spell;
        }

        protected virtual void OnDestroy()
        {

        }

        /// <summary>
        /// Spell Instantatie position, default set to caster LaunchPosition
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 SetSpawnPosition()
        {
            return caster.LaunchPoint.position;
        }

        /// <summary>
        /// Spell Instatniate rotation, default set to Quaternion.identity
        /// </summary>
        /// <returns></returns>
        protected virtual Quaternion SetSpawnRotation()
        {
            return Quaternion.identity;
        }
      
        protected virtual void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        protected virtual void Update()
        {

        }

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            Debug.Log(col.transform.name);
            Character _temp = col.transform.GetComponent<Character>();
            if(_temp) _temp.ApplyDamage(spellDMG);

        }

        public Sprite getIcon()
        {
            return spellBookIcon;
        }
    }
}