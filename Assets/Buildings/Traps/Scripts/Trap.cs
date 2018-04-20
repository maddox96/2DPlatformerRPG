using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public abstract class Trap : MonoBehaviour, IBuyable, IIconBarGeneratable
    {


        public void Buy()
        {
            BuildManager.instance.BuildPreview(this);
        }

        [Serializable]
        public class Positions
        {
            public Utility.possiblePositions positionEnum;

            Vector2 target { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }
            public Vector2 position { get { return Utility.GetWallPosition(target, positionEnum); } }
            public Ground ground { get { return Utility.GetWall(target, positionEnum); } }
            public float lenght { get { return Vector2.Distance(target, position); } }
        }

        [SerializeField]
        Positions[] positions;

        [SerializeField]
        private Sprite icon;

        public Transform groundTransform;
        public Rigidbody2D rb;
        public SpriteRenderer[] spriteRenderers;
        public ListOfColliders BaseCollidingWith;
        public List<ListOfColliders> groundColliders;
        public Positions closestPosition;
        public Utility.possiblePositions facedPosition;

        public bool DestroyAfterAction;
        Transform collisionCheckTransform;
        bool _possibleToBuild = false;
        bool _isPreview = true;
        public bool isPreview
        {

            get
            {
                return _isPreview;
            }

            set
            {
                _isPreview = value;
                if (isPreview == false)
                    OnBuild();
            }
        }

    

        protected int damage = 10;

        protected virtual void Action() { }
        protected virtual void Action(Character c) { }

        public Positions ClosestPosition()
        {
            if(positions.Length == 0)
            {
                Error.FatalError(gameObject, "you cant place this trap (avaliable postitions are not set!)");
                return null;
            }

            float lenght = positions[0].lenght;
            Positions _temp = positions[0];

            foreach (Positions p in positions)
            {
                if (p.lenght < lenght)
                {
                    lenght = p.lenght;
                    _temp = p;
                }
            }
            return _temp;
        }

        [SerializeField]
        private int _cost;

        public int cost
        {
            get
            {
                return _cost;
            }
        }

        protected virtual void Start()
        {
            _possibleToBuild = false;
        }

        protected virtual void Awake()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();

            collisionCheckTransform = Utility.GetChildrenWithTag(transform, "CollisionCheck");
            if(Error.FatalComponentNullCheck(gameObject, collisionCheckTransform))
                return;

            groundColliders = Utility.GetComponentInChildrensWithTag<ListOfColliders>(collisionCheckTransform, "GroundChecker");
            if (groundColliders.Count == 0)
                Error.FatalError(gameObject, "ground colliders has not been found");

            BaseCollidingWith = Utility.GetChildrenWithTag(collisionCheckTransform, "BaseChecker").GetComponent<ListOfColliders>();

        }

       

        public Sprite getIcon()
        {
            return icon;
        }

        protected virtual void OnBuild()
        {

        }


        public void PerformAction()
        {
            if (!isPreview)
            {
                Action();
                Destroy();
            }
           
        }

        public void PerformAction(Character c)
        {
            if (!isPreview)
            {
                Action(c);
                Destroy();
            }
        }


        void Destroy()
        {
            if (DestroyAfterAction)
                Destroy(gameObject);
        }

        
    }
}