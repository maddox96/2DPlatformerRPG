using UnityEngine;
using System.Collections;

namespace Portfolio
{
    public abstract class Projectile : MonoBehaviour
    {
        #region EDITOR PROPERTIES
        [Header("Projectile settings")]
        [Tooltip("Should the projectile collide with the ground layer (if not it will fly straight through)")]
        public bool ShouldCollideWithGround;
        [Tooltip("Should the projectile fall to the ground when it hits a wall")]
        public bool ShouldFallWhenCollided = false;
        [Tooltip("Should the projectile disapear when hit an enemy")]
        public bool ShouldBeDestroyedWhenCollided = true;
        [Tooltip("Layer that contains all the 'ground' colliders (if left as nothing it will be set from the character)")]
        public LayerMask OverrideGroundLayer;
        [Tooltip("A ParticleSystem to play when a collision occurs")]
        public ParticleSystem CollideEmitter;
        [Tooltip("Should the projectile be mirrored when travelling the opposite direction")]
        public bool ShouldFlipDirection = true;
        [Tooltip("The speed to move the projectile")]
        public int Speed = 500;
        [Tooltip("The speed to rotate the projectile")]
        public int RotationSpeed = 0;
        #endregion


        #region MEMBERS
        protected Character _owner;
        [HideInInspector]
        public Character Owner { get { return _owner; } set { _owner = value; IgnoreOwnerCollisions(this, value); } }
        protected Vector2 Direction { get; set; }
        protected Rigidbody2D rigidBody;
        protected SpriteRenderer Sprite;
        protected Vector3 RotationPoint = new Vector3();

        protected bool _isStopped;
        protected bool isStopped
        {
            get { return _isStopped; }
            set
            {
                _isStopped = value;
                if (value == false) return;
                rigidBody.velocity = Vector2.zero;
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        [HideInInspector]
        public int Damage;
        protected float lifeTime = 10.0f;
        #endregion

        #region ABSTRACT METHODS
        protected abstract void ApplyForce();
        protected abstract Vector2 SetDirection(Character owner);
        protected abstract Vector2 SetDirection();
        protected abstract void FlipRender();
        #endregion

        #region SPAWN METHODS
        public Projectile Shoot(Character shooter)
        {
            Projectile projectile = Instantiate(this, shooter.LaunchPoint.position, Quaternion.identity);
            projectile.Owner = shooter;
            projectile.Damage = shooter.WeaponDamage;
            projectile.SetLifeTime(projectile.lifeTime);
            return projectile;
        }

        public Projectile Shoot(int damage, Vector3 startPosition)
        {
            Projectile projectile = Instantiate(this, startPosition, Quaternion.identity);
            projectile.SetLifeTime(projectile.lifeTime);
            projectile.Damage = damage;
            return projectile;
        }       
 
        #endregion

        #region VIRTUAL METHODS

        protected virtual void ApplyRotation()
        {
            Sprite.transform.RotateAround(transform.position + RotationPoint, Vector3.forward, Time.deltaTime * -RotationSpeed * (Direction.x - Direction.y));
        }

        private void Awake()
        {
           

        }

        protected virtual void Start()
        {

            rigidBody = GetComponent<Rigidbody2D>();
            CollideEmitter = GetComponentInChildren<ParticleSystem>();
            Sprite = GetComponentInChildren<SpriteRenderer>();
            if (Owner != null) Direction = SetDirection(Owner);
            else Direction = SetDirection();

            if (ShouldFlipDirection) FlipRender();
            ApplyForce();
            ApplyRotation();
        }

        protected virtual void FixedUpdate()
        {
            if (RotationSpeed != 0 && Sprite != null && !isStopped)
                ApplyRotation();
        }

        protected virtual void Update()
        {
        }
        #endregion

        #region COLLISION METHODS

        protected virtual void OnTriggerEnter2D(Collider2D c)
        {
            if (!isGround(c.gameObject.layer))
            {
                Character character = c.transform.GetComponent<Character>();
                if (character != null)
                {
                    float direction = transform.position.x - character.transform.position.x;
                    character.ApplyDamage(Damage, direction);
                }
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D c)
        {

            Debug.Log(c.transform.name);
            if (isStopped)
                return;

            
            if(ShouldBeDestroyedWhenCollided)
            {
                /*
                fake destroy
                just to give children time to do effect stuff like
                trail renderer or particle.
                */
                turnOffSprite();
                turnOffMovement();
                turnOffColiders();

                if (CollideEmitter != null)   
                    CollideEmitter.Play(); 
            }


            Character character = c.transform.GetComponent<Character>();
            if (character != null)
            {
                float direction = c.contacts[0].point.x - character.transform.position.x;
                character.ApplyDamage(Damage, direction);
            }



            /*LayerMask ground = (OverrideGroundLayer == 0 && Owner != null ? Owner.GroundLayer : OverrideGroundLayer);
            Debug.Log(c.gameObject.layer);
            Debug.Log(ground.value);
            bool isOnGround = (ground & (1 << c.gameObject.layer)) != 0;

            if (isOnGround)
            {
                if (ShouldCollideWithGround)
                {
                   
                    Sprite.enabled = false;
                    if (ShouldFallWhenCollided)
                    {
                        if (rigidBody != null)
                        {
                            rigidBody.constraints = RigidbodyConstraints2D.None;
                        }
                    }
                    else
                    {
                        if (rigidBody != null)
                        {
                            rigidBody.isKinematic = true;
                        }

                        Collider2D[] colliders = this.GetComponentsInChildren<Collider2D>();
                        for (int i = 0; i < colliders.Length; i++)
                        {
                            colliders[i].enabled = false;
                        }
                    }

                    if (this.CollideEmitter != null)
                    {
                        this.CollideEmitter.Play();
                    }

                    isStopped = true;
                }
                else
                {
                    Collider2D[] projectileColliders = this.GetComponentsInChildren<Collider2D>();
                    for (int i = 0; i < projectileColliders.Length; i++)
                    {
                        Physics2D.IgnoreCollision(projectileColliders[i], c.collider);
                    }
                }
            }
            else
            {
                if (this.CollideEmitter != null)
                {
                    this.CollideEmitter.Play();
                }
                //GameObject.Destroy(this.gameObject);
            }

            // Apply damage to any character hit by this projectile
            

            */
        }

        bool isGround(LayerMask mask)
        {
            if (mask.value == 0) return true;
            else return false;
        }

        void turnOffColiders()
        {
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
        }

        void turnOffSprite()
        {
            Sprite.enabled = false;
        }

        void turnOffMovement()
        {
            isStopped = true;
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public static void IgnoreOwnerCollisions(Projectile projectile, Character owner)
        {
            // Prevent hitting the player who cast it
            if (owner != null)
            {
                Collider2D[] colliders = owner.GetComponentsInChildren<Collider2D>();
                Collider2D[] projectileColliders = projectile.GetComponentsInChildren<Collider2D>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    for (int j = 0; j < projectileColliders.Length; j++)
                    {
                        Physics2D.IgnoreCollision(colliders[i], projectileColliders[j]);
                    }
                }
            }
        }

        #endregion

        #region OTHER METODS
        protected void SetLifeTime(float lifeTime)
        {
            Destroy(gameObject, lifeTime);
        }

        #endregion
    }
}
