using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Portfolio
{
    public class LightningStrike : Spell
    {
        [SerializeField]
        GameObject lightningStorm;
        [SerializeField]

        LightningChain lightningChain;
        // EditorProperties
        [Header("Strike")]
        public float StrikeHeight = 3;
        public int ControlPoints;
        public float PatternChangeInterval;
        public float StrikeWidth;
        // Members
        private EdgeCollider2D collider;
        private LineRenderer strikeLine;
        private float changeTime;
        public override bool castAble
        {
            get
            {
                return true;
            }
        }

        public override string spellName
        {
            get
            {
                return "Lightning Strike";
            }
        }

        [HideInInspector]
        public HashSet<Character> characterHitted;

        protected override Vector3 SetSpawnPosition()
        {
            Vector3 _temp = Utility.GetWallPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _temp.z = 0.0f;
            return _temp;
        }

        public override Spell Cast(Vector3 target, Character caster = null)
        {
            Vector3 _temp = Utility.GetWallPosition(target);
            Spell spell = Instantiate(this, _temp, Quaternion.identity);
            spell.caster = caster;
            return spell;
        }

        protected override void Start()
        {
            if(isUpgradeBought(0))
            {
                if(castedTime > 3.0f)
                {
                    Instantiate(lightningStorm, transform.position + new Vector3(0.0f, 5.0f, 0.0f), Quaternion.identity);
                    Destroy(gameObject);
                }
            }

            characterHitted = new HashSet<Character>();
            base.Start();
            strikeLine = GetComponent<LineRenderer>();
            collider = GetComponentInChildren<EdgeCollider2D>();
            // We need to hide it until we have set the control points, 
            // otherwise it will be drawn in the wrong place
            strikeLine.enabled = false;
            if(isUpgradeBought(0))Instantiate(lightningChain, transform.position, Quaternion.identity, transform);
        }

        protected override void Update()
        {
            changeTime -= Time.deltaTime;

            if (changeTime <= 0)
            {
                UpdateStrikePattern();
                changeTime = PatternChangeInterval;
            }
        }

        private void UpdateStrikePattern()
        {
            // Set the starting position
            Vector2[] points = new Vector2[ControlPoints + 2];
            points[0] = target;
  
            // Generate a random position for each point
            for (int i = 1; i < points.Length; i++)
            {
                float x = target.x + Random.Range(-StrikeWidth, StrikeWidth);
                float y = ((float)i / (ControlPoints + 1)) * StrikeHeight;
                points[i] = new Vector2(x, target.y + y);
            }
            // Set the points on the line renderer
            //strikeLine.SetVertexCount(points.Length);  obsolete shiet
            strikeLine.positionCount = points.Length;

            collider.points = points;
            for (int i = 0; i < points.Length; i++)
            {
               
                strikeLine.SetPosition(i, points[i]);
            }
            // Show the renderer
            strikeLine.enabled = true;
        }

    }
}