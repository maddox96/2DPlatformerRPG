using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class Ground : MonoBehaviour
    {
        public Utility.possiblePositions[] trapPossiblePosition;

        public bool isPossibleToPutTrap(Utility.possiblePositions pos)
        {
            foreach(Utility.possiblePositions p in trapPossiblePosition)
            {
                if (pos == p)
                    return true;
            }

            return false;
        }
    }
}