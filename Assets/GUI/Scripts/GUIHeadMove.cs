using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class GUIHeadMove : MonoBehaviour
    {

        
        Transform playerTransform;
        Transform playerHeadTransform;

        [SerializeField]
        Transform GUIHeadTransform;
  

        // Use this for initialization
        void Start()
        {

            playerTransform = Utility.GetPlayer().GetComponent<Transform>();
            Transform[] childs = playerTransform.GetComponentsInChildren<Transform>();
            foreach(Transform t in childs)
            {
                if (t.name == "Head")
                    playerHeadTransform = t;

            }
        }

        // Update is called once per frame
        void Update()
        {
            if (playerHeadTransform == null) return;
            GUIHeadTransform.localPosition = playerHeadTransform.localPosition;
            GUIHeadTransform.localRotation = playerHeadTransform.localRotation;
        }
    }
}