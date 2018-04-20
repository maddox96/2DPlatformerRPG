using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{
    public class ColorChanger : MonoBehaviour
    {
        // its bugged when we add colorchange with the same duration on the same frame. 
        public Color color;
        SpriteRenderer[] sprites;

        private void Awake()
        {
            sprites = transform.parent.GetComponentsInChildren<SpriteRenderer>();
        }

        void Start()
        {
            SetColorOfSprites(color);
        }

        void SetColorOfSprites(Color color)
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = color;
            }
        }

        private void OnDestroy()
        {
            ColorChanger _temp = transform.parent.GetComponentInChildren<ColorChanger>();
            if (_temp != this && _temp != null)
                SetColorOfSprites(_temp.color);
            else 
                SetColorOfSprites(new Color(255, 255, 255));
        }
    }
}