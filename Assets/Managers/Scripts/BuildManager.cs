using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{


    public class BuildManager : MonoBehaviour
    {

        public static BuildManager instance;

        [SerializeField]
        Material RedBuildingMaterial;
        [SerializeField]
        Material GreenBuildingMaterial;
        [SerializeField]
        Material DefaultMaterial;

        Trap.Positions closestPosition;
        Vector2 mousePosition;

        Trap _currentBuilding;
        Trap currentBuilding
        {
            set
            {
                _currentBuilding = value;
                _possibleToBuild = false;
                if (currentBuilding != null)
                    foreach (SpriteRenderer s in currentBuilding.spriteRenderers)
                        s.material = RedBuildingMaterial;
            }

            get
            {
                return _currentBuilding;
            }
        }

        bool _possibleToBuild;
        public bool possibleToBuild
        {
            get
            {
                return _possibleToBuild;
            }

            set
            {
                if (value == true && possibleToBuild == false)
                {
                    foreach (SpriteRenderer s in currentBuilding.spriteRenderers)
                        s.material = GreenBuildingMaterial;
                }
                else if (value == false && possibleToBuild == true)
                {
                    foreach (SpriteRenderer s in currentBuilding.spriteRenderers)
                        s.material = RedBuildingMaterial;
                }

                _possibleToBuild = value;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        void Build()
        {
            if (currentBuilding != null)
            {
                currentBuilding.closestPosition = currentBuilding.ClosestPosition();
                currentBuilding.facedPosition = currentBuilding.closestPosition.positionEnum;
                currentBuilding.groundTransform = currentBuilding.ClosestPosition().ground.transform;

                foreach (SpriteRenderer s in currentBuilding.spriteRenderers)
                    s.material = DefaultMaterial;

                currentBuilding.isPreview = false;
                currentBuilding = null;
            }
        }

        bool collisionCheck()
        {

            float height = 0;
            bool heightSet = false;

            if (closestPosition.lenght == 0.0f) return false;

            foreach (ListOfColliders c in currentBuilding.groundColliders)
            {
                if (c.isColliding == false)
                {
                    Debug.Log("Building is floating in the air");
                    return false;
                }

            }

            foreach (Transform t in currentBuilding.BaseCollidingWith.colliders)
            {

                if (t == null) continue;
                if (t.tag != "Ground")
                {
                    Debug.Log("Building is with no gru");
                    return false;
                }

                if (t.tag == "Ground")
                {
                    if (!heightSet)
                    {
                        heightSet = true;
                        height = t.transform.position.y;
                    }

                    if (height != t.transform.position.y)
                    {
                        Debug.Log("2 highs");
                        return false;
                    }
                }
            }

            return true;
        }

        public void FreeCurrentBuilding()
        {
            if (currentBuilding != null)
            {
                Destroy(currentBuilding.gameObject);
                currentBuilding = null;
            }
        }

        private void FixedUpdate()
        {

            if (currentBuilding == null) return;
            if (Input.GetMouseButtonDown(0) && possibleToBuild && currentBuilding != null) Build();

            closestPosition = currentBuilding.ClosestPosition();
            possibleToBuild = collisionCheck();
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            if (closestPosition == null) return;
            if (closestPosition.lenght < 2.0f && closestPosition.ground.isPossibleToPutTrap(closestPosition.positionEnum))
            {
                currentBuilding.rb.MovePosition(closestPosition.position);
                currentBuilding.rb.MoveRotation(Utility.groundPositionToZRotation(closestPosition.positionEnum));
            }
            else
            {
                possibleToBuild = false;
                currentBuilding.rb.MovePosition(mousePosition);
            }
        }


        public void BuildPreview(Trap building)
        {
            FreeCurrentBuilding();
            currentBuilding = Instantiate(building);
        }
    }
}