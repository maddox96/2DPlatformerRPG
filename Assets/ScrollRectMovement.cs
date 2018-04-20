using UnityEngine;
using UnityEngine.UI;

namespace Portfolio
{
    public class ScrollRectMovement : MonoBehaviour {

        ScrollRect scrollRect;
        Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void ChangeScrollRectPosition()
        {
            if (scrollRect != null)
                scrollRect.horizontalNormalizedPosition = slider.value; 
        }
    }
}