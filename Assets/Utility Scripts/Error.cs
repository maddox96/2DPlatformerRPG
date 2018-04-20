using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class Error
    {

        public static void FatalError(GameObject o, string message)
        {
            Debug.LogError("Fatal error, " + o.name + "will be destroyed because, " + message);
            Object.Destroy(o);
        }

        public static bool FatalComponentNullCheck(GameObject o, Component c)
        {
            if (c != null) return false;
            Debug.LogError("Some component has been not attached to " + o.name);
            Object.Destroy(o);
            return true;
        }
    
    }
}