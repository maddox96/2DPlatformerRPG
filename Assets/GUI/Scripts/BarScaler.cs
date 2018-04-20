using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio
{
    public abstract class BarScaler : MonoBehaviour
    {

        protected Character character;
        [SerializeField]
        float scaleSpeed;
        RectTransform barTransformRect;

        float barWidth;
        float scaleDestiny;
        float currentScale;

        protected abstract float currentRelativeStat { get; }
        protected abstract float maxRelativeStat { get; }
        protected abstract void ScaleBarEvent();

        void Start()
        {
            character = Utility.GetPlayer();
            if (character == null)
                Debug.LogError("Bar scaler has not found the player");
            ScaleBarEvent();
            barTransformRect = GetComponent<RectTransform>();
            barWidth = barTransformRect.rect.width;           
            scaleDestiny = barWidth * (currentRelativeStat / maxRelativeStat);
        }

        protected void scaleBar()
        {
            currentScale = barTransformRect.sizeDelta.x;
            scaleDestiny = barWidth * (currentRelativeStat / maxRelativeStat);
        }

        void Update()
        {
            if(barTransformRect.sizeDelta.x > scaleDestiny)
            {
                barTransformRect.sizeDelta = new Vector2(barTransformRect.sizeDelta.x - Time.deltaTime * scaleSpeed, barTransformRect.sizeDelta.y);
            }
        }
    }

}