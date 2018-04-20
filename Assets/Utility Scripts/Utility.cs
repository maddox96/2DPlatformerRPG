using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public static class Utility {

        public enum Direction { UP, LEFT, RIGHT, DOWN };
        public enum possiblePositions { GROUND, LEFT, RIGHT, CEILING };
        public static readonly Vector2 GROUND_NOT_FOUND = new Vector2(-999.0f, -999.0f);



        public static Direction PossiblePositionToDirection(possiblePositions p)
        {
            switch(p)
            {
                case possiblePositions.CEILING: return Direction.DOWN;
                case possiblePositions.GROUND: return Direction.UP;
                case possiblePositions.LEFT: return Direction.LEFT;
                default: return Direction.RIGHT;         
            }
        }
        public static float groundPositionToZRotation(possiblePositions pos)
        {
            switch(pos)
            {
                case possiblePositions.CEILING: return 180.0f;
                case possiblePositions.LEFT: return 90.0f;
                case possiblePositions.RIGHT: return -90.0f;
                default:  return 0.0f;
            }
        }

        public static float DirectionToZRotation(Direction pos)
        {
            switch (pos)
            {
                case Direction.DOWN: return -90.0f;
                case Direction.LEFT: return 180.0f;
                case Direction.UP: return 90.0f;
                default: return 0.0f;
            }
        }


        public static Vector2 DirectionToVector(Direction d)
        {
            switch (d)
            {
                case Utility.Direction.UP:
                    return Vector2.up;
                case Utility.Direction.DOWN:
                    return Vector2.down;
                case Utility.Direction.LEFT:
                    return Vector2.left;
                default:
                    return Vector2.right;
            }
        }
        public static Vector2 possiblePositionsToVector(possiblePositions pos)
        {
            switch (pos)
            {
                case possiblePositions.CEILING: return Vector2.up;
                case possiblePositions.GROUND: return Vector2.down;
                case possiblePositions.LEFT: return Vector2.right; // its not a mistake
                case possiblePositions.RIGHT: return Vector2.left;
                default: return Vector2.zero;
            }
        }


        public static Vector2 GetWallPosition(Vector2 from, possiblePositions pos = possiblePositions.GROUND)
        {
            Vector2 direction = possiblePositionsToVector(pos);
            RaycastHit2D[] rays;
            rays = Physics2D.RaycastAll(from, direction);

            foreach (RaycastHit2D hit in rays)
            {
                if (hit.collider.tag == "Ground")
                {
                    return (direction == Vector2.down || direction == Vector2.up) ?
                        new Vector2(hit.point.x, hit.point.y) :
                        new Vector2(hit.point.x, hit.point.y);     
                }
            }

            return GROUND_NOT_FOUND;

        }

        public static Ground GetWall(Vector2 from, possiblePositions pos)
        {
            Vector2 direction = possiblePositionsToVector(pos);
            RaycastHit2D[] rays;
            rays = Physics2D.RaycastAll(from, direction);

            foreach (RaycastHit2D hit in rays)
            {
                if (hit.collider.tag == "Ground")
                {
                    return hit.collider.GetComponent<Ground>();
                }
            }

            return new Ground();
        }
       
        public static Character GetPlayer()
        {
            Character c = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            if (c == null) Debug.LogError("PLAYER NOT FOUND");
            return c;
        }

        public static T GetComponentInChildrenWithoutParent<T>(Transform t)
        {
            T[] _temp;
            _temp = t.GetComponentsInChildren<T>();
            return _temp[1];
        }

        public static void DestoryChildrens(Transform T)
        {
            int childCount = T.childCount;
            for(int i = 0; i < childCount; i++)
            {
                Object.Destroy(T.GetChild(i).gameObject);
            }
        }

        public static List<T> GetComponentInChildrensWithTag<T>(Transform transform, string tag)
        {
            List<Transform> childs = GetChildrensWithTag(transform, tag);
            List<T> components = new List<T>();
            T component;
            foreach (Transform c in childs)
            {
                component = c.GetComponent<T>();
                if(component != null)
                    components.Add(c.GetComponent<T>());
            }

            return components;
        }

        public static Transform GetChildrenWithTag(Transform T, string tag)
        {
            Transform _temp = null;
            for (int i = T.childCount - 1; i >= 0; i--)
            {

                _temp = T.GetChild(i);
                if (_temp == null) break;

                if (_temp.tag == tag)
                    return _temp;
            }
            return _temp;
        }


        public static List<Transform> GetChildrensWithTag(Transform T, string tag)
        {
            List<Transform> childs = new List<Transform>();
            Transform _temp;
            for (int i = T.childCount - 1; i >= 0; i--)
            {
               
                _temp = T.GetChild(i);
                if (_temp == null) break;

                if (_temp.tag == tag)
                    childs.Add(_temp);
            }

            return childs;
        }
    }
}