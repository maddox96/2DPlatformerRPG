using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Portfolio
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour
    {
        #region Editor Properties
        [Header("Character")]
        [Tooltip("Transform used to check if the character is touching the ground")]
        public Transform GroundChecker;
        [Tooltip("Layer that contains all the 'ground' colliders")]
        public LayerMask GroundLayer;
        [Tooltip("Speed of the character when running")]
        public float RunSpeed = 250;
        [Tooltip("Factor of run speed to lose(-)/gain(+) when pressing the modifier. Use for run boost or sneak.")]
        [Range(-1, 1)]
        public float RunModifierFactor = -0.75f;
        [Tooltip("Factor of velocity to lose(-)/gain(+) when blocking (if enabled)")]
        [Range(-1, 1)]
        public float BlockingMoveFactor = -0.75f;
        [Tooltip("Should the character be allowed to double jump")]
        public bool AllowDoubleJump = true;
        [Tooltip("Should the character be allowed to control direction while in the air")]
        public bool AllowAirControl = true;
        [Tooltip("Force applied to character when pressing jump")]
        public float JumpPower = 550;
        [Tooltip("Should the character be allowed to jump while sliding down a wall")]
        public bool AllowWallJump = true;
        [Tooltip("Transform used to check if the character is sliding down a wall forwards")]
        public Transform WallCheckerFront;
        [Tooltip("Transform used to check if the character is sliding down a wall backwards")]
        public Transform WallCheckerBack;
        [Tooltip("Layer that contains all the 'wall' colliders")]
        public LayerMask WallLayer;
        [Tooltip("Factor of velocity to lose(-)/gain(+) when sliding down a wall")]
        [Range(-1, 1)]
        public float WallSlideFactor = -0.25f;
        [Tooltip("Force applied to character horizontally when jumping off a wall")]
        public float WallJumpHorizontalPower = 100;
        [Tooltip("Allow the character to jump down oneway platforms (containing a PlatformEffector2D with oneway set to true)")]
        public bool AllowJumpDownPlatforms = true;
        [Tooltip("The time to wait after jumping down before re-enabling the platform")]
        public float JumpDownTimeout = 1;
        [Tooltip("Allow the character to climb ladder trigger colliders (named or tagged 'ladder')")]
        public bool UseLadders = true;
        [Tooltip("Allow the character to move slower in water trigger colliders (named or tagged 'water')")]
        public bool UseWater = true;
        [Tooltip("Factor of velocity to lose(-)/gain(+) when moving in water")]
        [Range(-1, 1)]
        public float WaterMoveFactor = -0.75f;
        [Tooltip("Allow the character to slide on ice trigger colliders (named or tagged 'ice')")]
        public bool UseIce = true;
        [Tooltip("Amount of friction to use for ice")]
        public float IceFriction = 1f;
        [Tooltip("Should the character ignore all the mecanim animation states and remain static (useful for character select screens)")]
        public bool IgnoreAnimationStates = false;
        [Tooltip("Health of the character")]
        public int MaxHealth = 100;
        [Tooltip("Mana of the character")]
        public int MaxMana = 100;
        [Tooltip("Gravity scale applied to the RigidBody2D on start up")]
        public float GravityScale = 3;
        [Tooltip("Is the character a zombie")]
        public bool IsZombified = false;
        [Tooltip("A particle system to spawn when entering water")]
        public ParticleSystem SplashEmitter;
        [Tooltip("A particle system to start when underwater")]
        public ParticleSystem BubbleEmitter;
        [Tooltip("A particle system to spawn when jumping")]
        public ParticleSystem DustEmitter;
        [Tooltip("The velocity the character must be travelling to create a dust cloud when landing")]
        public float DustCloudThreshold = -10;
        public int Money = 260;
        [Header("Weapon")]
        [Tooltip("Type of weapon character is carrying. Used in animations.")]
        public WeaponType EquippedWeaponType;
        [Tooltip("Can the character block?")]
        public bool IsBlockEnabled = false;
        [Tooltip("Transform position used as the spawn point for projectiles")]
        public Transform LaunchPoint;
        [Tooltip("Projectile to spawn when casting")]
        public Projectile CastProjectile;
        [Tooltip("Projectile to spawn when throwing from main hand")]
        public Projectile ThrowMainProjectile;
        [Tooltip("Projectile to spawn when throwing from off hand")]
        public Projectile ThrowOffProjectile;
        [Tooltip("Transform position used to spawn an effect")]
        public Transform EffectPoint;
        #endregion

        #region Script Properties
        public int CurrentHealth { get; private set; }
        public int CurrentMana { get; private set; }
        public float speedModifier = 5.0f;
        public bool IsDead { get { return CurrentHealth <= 0; } }
        public bool isCasting { get; set; }
        public Direction CurrentDirection { get; private set; }
        public Dictionary<Effects.BoolEffects, bool> currentCC;
        public int WeaponDamage;
        public Effects.BoolEffects immunity;
        public Profession profession;

        public float ModifiedSpeed
        {
            get
            {
                return RunSpeed * GetMultiplier(RunModifierFactor) * speedModifier;
            }
        }

        public bool IsAttacking
        {
            get
            {
                AnimatorStateInfo state = this.animatorObject.GetCurrentAnimatorStateInfo(3);
                return state.IsName("Attack") || state.IsName("Quick Attack");
            }
        }

        public enum WeaponType
        {
            None = 0,
            Staff = 1,
            Sword = 2,
            Bow = 3,
            Gun = 4
        }
           

        public enum Direction
        {
            Left = -1,
            Right = 1
        }

        [Flags]
        public enum Action
        {
            Jump = 1,
            RunModified = 2,
            QuickAttack = 4,
            Attack = 8,
            Cast = 16,
            ThrowOff = 32,
            ThromMain = 64,
            Consume = 128,
            Block = 256,
            Hurt = 512,
            JumpDown = 1024,
            Crouch = 2048,
            StopCasting = 4096
        }
        #endregion

        #region Members
        private Animator animatorObject;
        private Rigidbody2D body2D;
        private Spellbook spellbook;
        private bool isGrounded = true;
        private bool isOnWall = false;
        private bool isOnWallFront = false;
        private bool isOnLadder = false;
        private bool isInWater = false;
        private bool isOnIce = false;
        private bool isBlocking = false;
        private bool isCrouching = false;
        private bool isKnockbacked = false;
        private bool isJumpPressed;
        private bool isJumpingDown;
        private bool isReadyForDust;
        private int jumpCount = 0;
        private bool isRunningNormal = false;
        private float groundRadius = 0.1f;
        private float wallDecayX = 0.006f;
        private float wallJumpX = 0;
        private float timeCasting = 0.0f;
        private Direction startDirection = Direction.Right;

        #endregion

        #region Delegates 

        public delegate void playerDelegates();
        public playerDelegates OnDamageTake;
        public playerDelegates OnManaSpend;
        public playerDelegates OnMoneySpend;

        #endregion

        #region Spawn methods

        public static Character Create(Character instance, Direction startDirection, Vector3 position)
        {
            Character c = GameObject.Instantiate<Character>(instance);
            c.transform.position = position;
            c.startDirection = startDirection;
            return c;
        }
        #endregion

        #region Init methods

        private void Awake()
        {
            CurrentMana = MaxMana;
            CurrentHealth = MaxHealth;
        }
        void Start()
        {
            profession = new Wizard();
            // init dictionary with every cc in the game
            currentCC = new Dictionary<Effects.BoolEffects, bool>();
            foreach (Effects.BoolEffects cc in Enum.GetValues(typeof(Effects.BoolEffects)))
            {
                currentCC.Add(cc, false); 
            }

            body2D = GetComponent<Rigidbody2D>();
            animatorObject = GetComponent<Animator>();
            spellbook = GetComponent<Spellbook>();

            // Apply the gravity scale because 2D physics jumping look too floaty without extra gravity
            if (body2D != null)
            {
                body2D.gravityScale = GravityScale;
            }

            CurrentHealth = MaxHealth;
            CurrentMana = MaxMana;
            ApplyDamage(0);
            body2D.centerOfMass = new Vector2(0f, 0.4f);

            if (startDirection != Direction.Right)
            {
                ChangeDirection(startDirection);
            }
            else
            {
                CurrentDirection = startDirection;
            }

            // Setup the underwater FX
            if (BubbleEmitter != null && BubbleEmitter.isPlaying)
            {
                BubbleEmitter.Stop();
                BubbleEmitter.gameObject.SetActive(false);
            }

            // Warn the user if they have forgotten to setup the layers correctly
            if ((GroundLayer & (1 << gameObject.layer)) != 0)
            {
                Debug.LogWarningFormat(this, "The character has its GroundLayer set incorrectly.\r\nThe GroundLayer matches the Character's main Layer, so it will not jump/fall correctly\r\nPlease update either the GroundLayer or the Layer of the character.");
            }

            // Perform an initial ground check
            isGrounded = CheckGround();

        }
        #endregion

        #region Update methods

        private void Update()
        {
            if (isCasting) timeCasting += Time.deltaTime;
        }

        void FixedUpdate()
        {
            // Check if we are touching the ground using the rigidbody
            isGrounded = CheckGround();

            // Check if we are touching a wall when wall jump is allowed
            bool isOnWallFront = false;
            bool isOnWallBack = false;
            if (AllowWallJump && !isGrounded && body2D.velocity.y <= 0)
            {
                isOnWallFront = Physics2D.OverlapCircle(WallCheckerFront.position, groundRadius, WallLayer);
                isOnWallBack = Physics2D.OverlapCircle(WallCheckerBack.position, groundRadius, WallLayer);
            }

            isOnWall = (isOnWallFront || isOnWallBack);
            isOnWallFront = (isOnWall && isOnWallFront);
        }
        #endregion
       
        #region Collision methods
        void OnCollisionEnter2D(Collision2D c)
        {
            // Check if we hit the ground (going downwards)
            if ((GroundLayer.value & 1 << c.gameObject.layer) != 0 &&
                c.contacts[0].normal.y > 0.8f)
            {
                // If we did, check to see if we either just jumped or are falling fast enough for dust
                if (isReadyForDust || body2D.velocity.y < DustCloudThreshold)
                {
                    CreateDustCloud(c.contacts[0].point);
                }

                // We hit the ground after jumping
                isReadyForDust = false;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Check if they are splashing into water
            if (UseWater && IsTrigger(other.gameObject, "water"))
            {
                Vector2 pos = GroundChecker.position;
                pos.y = other.bounds.center.y + other.bounds.extents.y;
                CreateSplash(pos);

                // Play bubbles
                if (BubbleEmitter != null && !BubbleEmitter.isPlaying)
                {
                    BubbleEmitter.gameObject.SetActive(true);
                    BubbleEmitter.Play();
                }
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            // Check for ladders
            if (UseLadders && IsTrigger(other.gameObject, "ladder"))
            {
                body2D.isKinematic = true;
                isOnLadder = true;
                animatorObject.SetBool("IsOnLadder", isOnLadder);
            }

            // Check for water
            if (UseWater && IsTrigger(other.gameObject, "water"))
            {
                isInWater = true;
                animatorObject.SetBool("IsInWater", isInWater);
            }

            // Check for ice
            if (UseIce && IsTrigger(other.gameObject, "ice"))
            {
                isOnIce = true;
                animatorObject.SetBool("IsOnIce", isOnIce);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // Check for ladders
            if (UseLadders && IsTrigger(other.gameObject, "ladder"))
            {
                body2D.isKinematic = false;
                isOnLadder = false;
                animatorObject.SetBool("IsOnLadder", isOnLadder);
            }

            // Check for water
            if (UseWater && IsTrigger(other.gameObject, "water"))
            {
                isInWater = false;
                animatorObject.SetBool("IsInWater", isInWater);

                if (BubbleEmitter != null && BubbleEmitter.isPlaying)
                {
                    BubbleEmitter.Stop();
                    BubbleEmitter.gameObject.SetActive(false);
                }
            }

            // Check for ice
            if (UseIce && IsTrigger(other.gameObject, "ice"))
            {
                isOnIce = false;
                animatorObject.SetBool("IsOnIce", isOnIce);
            }
        }
        private Collider2D CheckGround()
        {
            // Check if we are touching the ground using the rigidbody
            return Physics2D.OverlapCircle(GroundChecker.position, groundRadius, GroundLayer);
        }

        private bool IsTrigger(GameObject other, string name)
        {
            name = name.ToLower();

            if ((other.tag != null && other.tag.ToLower() == name) ||
                (other.name != null && other.name.ToLower() == name))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Movement methods

        /// <summary>
        /// Perform movement for the character
        /// This should be called from the FixedUpdate() method as it makes changes to physics properties
        /// </summary>
        /// <param name="axis">The x and y values used for running and climbing ladders</param>
        /// <param name="isHorizontalStillPressed">True if the user is still actively pressing the horizontal axis</param>

        public void Move(Vector2 axis, bool isHorizontalStillPressed)
        {
            // Quit early if dead
            if (IsDead || currentCC[Effects.BoolEffects.Stun] || currentCC[Effects.BoolEffects.Freeze])
            {
                body2D.velocity = new Vector2(0, body2D.velocity.y);
                return;
            }

            if (IgnoreAnimationStates)
            {
                isGrounded = true;
                animatorObject.SetBool("IsGrounded", isGrounded);
                return;
            }

            if(isKnockbacked)
            {
                float horizontal = axis.x;
                if ((horizontal < 0.0 && body2D.velocity.x > 0.0f) || horizontal > 0.0 && body2D.velocity.x < 0.0f) horizontal *= 3.0f;
                float speed = (isRunningNormal ? RunSpeed : ModifiedSpeed);
                Vector2 newVelocity = new Vector2(horizontal * speed * Time.deltaTime, body2D.velocity.y);
                body2D.AddForce(newVelocity, ForceMode2D.Force);
                isKnockbacked = !CheckGround();
            }

            // Get the input and speed
            if ((isGrounded || AllowAirControl) && !isKnockbacked)
            {

                float horizontal = axis.x;

                // Check if we are jumping off a wall when using AirControl
                if (AllowWallJump && !isGrounded && !Mathf.Approximately(wallJumpX, 0))
                {
                    if (!isHorizontalStillPressed)
                    {
                        // Wall jumps with AirControl emulate a user pressing the direction,
                        // so that they jump off at an angle. You can't just rely on the 
                        // rigidbody force since we overwrite it with the user input.
                        wallJumpX = Mathf.Lerp(wallJumpX, 0, Time.deltaTime);
                        horizontal = wallJumpX;
                    }
                    else
                    {
                        wallJumpX = 0;
                    }
                }

                // Set the new velocity for the character based on the run modifier
                float speed = (isRunningNormal ? RunSpeed : ModifiedSpeed);
                Vector2 newVelocity = new Vector2(horizontal * speed * Time.deltaTime, body2D.velocity.y);
                if (currentCC[Effects.BoolEffects.Fear]) newVelocity.x *= -1;
                if (isOnIce)
                {
                    // If on ice we should slide
                    newVelocity.x = Mathf.Lerp(body2D.velocity.x, newVelocity.x, IceFriction * Time.deltaTime);
                }

                //body2D.AddForce(newVelocity, ForceMode2D.Impulse);
                body2D.velocity = newVelocity; // need to be addforce
            }

            // If they pressed jump, then add some Y velocity
            if (isJumpPressed)
            {
                float xPower = 0;
                if (AllowWallJump && isOnWall)
                {
                    isKnockbacked = false;
                    // Add horizontal power for wall jumps
                    xPower = WallJumpHorizontalPower * (int)CurrentDirection;
                    wallJumpX = xPower * wallDecayX;

                    // Show the dust cloud when we jump off a wall
                    CreateDustCloud(GroundChecker.position);
                }
                else
                {
                    wallJumpX = 0;
                }

                body2D.velocity = new Vector2(body2D.velocity.x, 0);
                body2D.AddForce(new Vector2(xPower, JumpPower));
                isJumpPressed = false;
                isReadyForDust = true;

                // If we are double jumping, play an extra rolling animation,
                // so that the jump looks cooler.
                if (jumpCount == 2)
                {
                    animatorObject.Play("Roll", LayerMask.NameToLayer("FX"));
                }
            }
            else if (isOnLadder)
            {
                Debug.Log("We are on ladder, lol");
                // Ladder climbing means we use the Y axis
                float vertical = axis.y;
                body2D.velocity = new Vector2(body2D.velocity.x, vertical * RunSpeed * Time.deltaTime);
            }
            else if (isOnWall && !isGrounded)
            {
                isKnockbacked = false;
                // If they are sliding down a wall, slow them down
                if (body2D.velocity.y < 0 &&
                    (body2D.velocity.x >= 0 && CurrentDirection < 0 ||
                     body2D.velocity.x <= 0 && CurrentDirection > 0))
                {
                    body2D.velocity = new Vector2(body2D.velocity.x, body2D.velocity.y * GetMultiplier(WallSlideFactor));
                }
            }
            else if (isBlocking)
            {
                // Blocking changes the speed of the character
                body2D.velocity = new Vector2(body2D.velocity.x * GetMultiplier(BlockingMoveFactor), body2D.velocity.y);
            }

            if (isInWater)
            {
                // Water also changes the speed of the character
                float waterFactor = GetMultiplier(WaterMoveFactor);
                body2D.velocity = new Vector2(body2D.velocity.x * waterFactor, body2D.velocity.y);
            }

            // Update the animator
            animatorObject.SetBool("IsGrounded", isGrounded);
            animatorObject.SetBool("IsOnWall", isOnWall);
            animatorObject.SetInteger("WeaponType", (int)EquippedWeaponType);
            animatorObject.SetBool("IsZombified", IsZombified);
            animatorObject.SetFloat("AbsY", Mathf.Abs(body2D.velocity.y));
            animatorObject.SetFloat("VelocityY", body2D.velocity.y);
            animatorObject.SetFloat("VelocityX", Mathf.Abs(body2D.velocity.x));
            animatorObject.SetBool("HasMoveInput", isHorizontalStillPressed);
            animatorObject.SetBool("IsCrouching", isCrouching && isGrounded);

            // Flip the sprites if necessary
            if (isOnWall)
            {
                if (isOnWallFront && !isOnLadder)
                {
                    ChangeDirection(CurrentDirection == Direction.Left ? Direction.Right : Direction.Left);
                }
            }
            else if (axis.x != 0)
            {
                ChangeDirection(axis.x < 0 ? Direction.Left : Direction.Right);
            }
        }

        /// <summary>
        /// Perform the specified actions for the character
        /// </summary>
        /// <param name="action">A combined set of flags for all the actions the character should perform</param>
        public void Perform(Action action)
        {
            // Quit early if dead
            if (IsDead)
            {
                return;
            }

            // Check if we are blocking
            isBlocking = IsAction(action, Action.Block) && IsBlockEnabled;

            // Check if we are crouching
            isCrouching = IsAction(action, Action.Crouch);

            // Check for the running modifier key
            isRunningNormal = !IsAction(action, Action.RunModified);

            // Reset the jump count if we are on the ground
            if (isGrounded)
            {
                jumpCount = 0;
            }

            // Check for jumping down since we need to remove the regular jump flag if we are
            if (IsAction(action, Action.JumpDown))
            {
                Collider2D ground = CheckGround();
                if (ground != null)
                {
                    PlatformEffector2D fx = ground.GetComponent<PlatformEffector2D>();
                    if (fx != null && fx.useOneWay)
                    {
                        ground.enabled = false;
                        action &= ~Action.Jump;

                        StartCoroutine(EnableAfter(JumpDownTimeout, ground));
                    }
                }
            }

            // Now check the rest of the keys for actions
            if (IsAction(action, Action.Jump) && !isJumpPressed)
            {
                // Prevent them jumping on ladders
                if (!isOnLadder)
                {
                    if (isGrounded || (AllowWallJump && isOnWall))
                    {
                        isJumpPressed = true;
                        jumpCount = 1;
                    }
                    else if (AllowDoubleJump && jumpCount <= 1)
                    {
                        isJumpPressed = true;
                        jumpCount = 2;
                    }
                }
            }
            else if (IsAction(action, Action.QuickAttack))
            {
                TriggerAction("TriggerQuickAttack");
            }
            else if (IsAction(action, Action.Attack))
            {
                TriggerAction("TriggerAttack");
            }
            else if (IsAction(action, Action.Cast))
            {
                if (spellbook == null || spellbook.currentToolbar == null) return;
                if (spellbook.currentToolbar.IsOnCooldown) return;
                if (spellbook.currentSpell.castAble)
                {
                    isCasting = true;
                    TriggerAction("TriggerCasting");
                }else TriggerAction("TriggerCast");
            }
            else if (IsAction(action, Action.ThrowOff))
            {
                TriggerAction("TriggerThrowOff");
            }
            else if (IsAction(action, Action.StopCasting))
            {
                if (!isCasting) return; 
                isCasting = false;
                OnCastEffect();
                timeCasting = 0.0f;
                animatorObject.SetBool("TriggerCasting", false);
            }
            else if (IsAction(action, Action.ThromMain))
            {
                TriggerAction("TriggerThrowMain");
            }
            else if (IsAction(action, Action.Consume))
            {
                TriggerAction("TriggerConsume");
            }
            else if (isBlocking && !animatorObject.GetBool("IsBlocking"))
            {
                TriggerAction("TriggerBlock");
            }
            else if (IsAction(action, Action.Hurt))
            {
                // Apply some damage to test the animation
                ApplyDamage(10);
            }

            // Reset the blocking animation if they let go of the block button
            if (!isBlocking)
            {
                animatorObject.SetBool("IsBlocking", isBlocking);
            }
        }

        private void TriggerAction(string action, bool isCombatAction = true)
        {
            // Update the animator object
            animatorObject.SetTrigger(action);
            animatorObject.SetBool("IsBlocking", isBlocking);

            if (isCombatAction)
            {
                // Combat actions also trigger an additional parameter to move correctly through states
                animatorObject.SetTrigger("TriggerCombatAction");
            }
        }

        private bool IsAction(Action value, Action flag)
        {
            return (value & flag) != 0;
        }
        private void ChangeDirection(Direction newDirection)
        {
            if (CurrentDirection == newDirection)
            {
                return;
            }

            // Swap the direction of the sprites
            Vector3 rotation = transform.localRotation.eulerAngles;
            rotation.y -= 180;
            transform.localEulerAngles = rotation;
            CurrentDirection = newDirection;

            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < sprites.Length; i++)
            {
                Vector3 position = sprites[i].transform.localPosition;
                position.z *= -1;
                sprites[i].transform.localPosition = position;
            }
        }

        private float GetMultiplier(float factor)
        {
            if (Mathf.Sign(factor) < 0)
            {
                return 1 + factor;
            }
            else
            {
                return factor;
            }
        }


        public void Knockback(Vector2 direction, float power)
        {
            isKnockbacked = true;
            body2D.velocity = Vector2.zero;
            body2D.AddForce(direction * power, ForceMode2D.Force);
        }

        #endregion

        #region HP and Mana apply methods
        public bool ApplyDamage(int damage, float direction = 0)
        {
            if (!this.IsDead)
            {
                this.animatorObject.SetFloat("LastHitDirection", direction * (int)this.CurrentDirection);

                // Update the health
                this.CurrentHealth = Mathf.Clamp(this.CurrentHealth - damage, 0, this.MaxHealth);
                this.animatorObject.SetInteger("Health", this.CurrentHealth);

                if (damage != 0)
                {
                    if (OnDamageTake != null) OnDamageTake();
                    // Show the hurt animation
                    this.TriggerAction("TriggerHurt", false);
                }

                if (this.CurrentHealth <= 0)
                {
                    // Since the player is dead, remove the corpse
                    StartCoroutine(this.DestroyAfter(1, this.gameObject));
                }
            }

            return this.IsDead;
        }

        public bool enoughMoney(IBuyable obj)
        {
            if (Money - obj.cost > 0)
                return true;

            return false;
        }
        public bool enoughMana(Spell spell)
        {
            if(CurrentMana - spell.manaCost > 0)   
                return true;

            return false;
        }

        public void spendMoney(IBuyable obj)
        {
            if(enoughMoney(obj))
                Money -= obj.cost;

            if(OnMoneySpend != null) OnMoneySpend();
        }

        public void DrainMana(int mana)
        {
            CurrentMana = Mathf.Clamp(CurrentMana - mana, 0, MaxMana);
            if(OnManaSpend != null) OnManaSpend();
        }

        public void Heal(int value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
        }
        #endregion
           
        #region OnAnimationComplete methods

        private void OnCastEffect()
        {
            spellbook.Cast(timeCasting);
        }
        
        private void OnCastEffectStop()
        {
         
        }

        private void OnCastComplete()
        {
            // Stop the active effect once we cast
           // this.OnCastEffectStop();

            // Create the projectile
            // LaunchProjectile(CastProjectile);
        }

        private void OnThrowMainComplete()
        {
            // Create the projectile for the main hand
            this.LaunchProjectile(this.ThrowMainProjectile);
        }

        private void OnThrowOffComplete()
        {
            // Create the projectile for the off hand
            this.LaunchProjectile(this.ThrowOffProjectile);
        }

        public void CancelCasting()
        {
            Debug.Log("canceling casting..");

            isCasting = false;
            timeCasting = 0.0f;
            animatorObject.SetBool("TriggerCasting", false);
        }

        private void LaunchProjectile(Projectile projectile)
        {
            // Create the projectile
            if (projectile != null)
            {
                projectile.Shoot(this);
            }
        }

        #endregion

        #region Particles methods
        private void CreateSplash(Vector2 point)
        {
            if (this.SplashEmitter != null)
            {
                // Create a cloud of dust
                ParticleSystem splash = GameObject.Instantiate<ParticleSystem>(this.SplashEmitter);
                splash.transform.position = point;
                splash.Play();
                StartCoroutine(this.DestroyAfter(splash.duration, splash.gameObject));
            }
        }

        private void CreateDustCloud(Vector2 point)
        {
            if (this.DustEmitter != null)
            {
                // Create a cloud of dust
                ParticleSystem dust = GameObject.Instantiate<ParticleSystem>(this.DustEmitter);
                dust.transform.position = point;
                dust.Play();
                StartCoroutine(this.DestroyAfter(dust.duration, dust.gameObject));
            }
        }

        #endregion
       
        #region Other methods
        private IEnumerator DestroyAfter(float seconds, GameObject gameObject)
        {
            yield return new WaitForSeconds(seconds);

            GameObject.Destroy(gameObject);
        }

        private IEnumerator EnableAfter(float seconds, Behaviour obj)
        {
            yield return new WaitForSeconds(seconds);

            obj.enabled = true;
        }

        #endregion


    }
}