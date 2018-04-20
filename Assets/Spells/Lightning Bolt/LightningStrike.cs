using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AnimatedPixelPack
{
    public class LightningStrike : Spell
    {
        // EditorProperties
        [Header("Strike")]
        public float StrikeHeight = 6;
        public int ControlPoints = 2;
        public float PatternChangeInterval = 0.05f;
        public float StrikeWidth = 1f;

        // Members
        private LineRenderer strikeLine;
        private float changeTime;

        protected override void Start()
        {
            lifeTime = 3.0f;
            base.Start();
            strikeLine = GetComponent<LineRenderer>();
            // We need to hide it until we have set the control points, 
            // otherwise it will be drawn in the wrong place
            strikeLine.enabled = false;
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
            Vector3[] points = new Vector3[this.ControlPoints + 2];
            points[0] = target;

            // Generate a random position for each point
            for (int i = 1; i < points.Length; i++)
            {
                float x = target.x + Random.Range(-StrikeWidth, StrikeWidth);
                float y = ((float)i / (ControlPoints + 1)) * StrikeHeight;
                points[i] = new Vector3(x, target.y + y, 0);
            }
            // Set the points on the line renderer
            //strikeLine.SetVertexCount(points.Length);  obsolete shiet
            strikeLine.numPositions = points.Length;

            for (int i = 0; i < points.Length; i++)
            {
                strikeLine.SetPosition(i, points[i]);
            }
            // Show the renderer
            strikeLine.enabled = true;
        }
    }
}