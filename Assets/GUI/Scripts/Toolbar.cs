using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio
{
    public class Toolbar : MonoBehaviour {

        private Text cooldownTimer;
        private float timeOnCd;
        public Image icon;

        public Spell assignedSpell;
        

        private void Awake()
        {
            cooldownTimer = transform.GetComponentInChildren<Text>();
            Image[] images = transform.GetComponentsInChildren<Image>();
            foreach(Image img in images)
            {
                if (img.transform != transform)
                    icon = img.GetComponent<Image>();
            }
        }

        void Start()
        {
            icon.sprite = assignedSpell.spellBookIcon;
        }

        public bool IsOnCooldown 
        {
            get
            {
                if (timeOnCd <= 0)
                {
                    return false;
                }
                else return true;
            }
        }
      
        public void SetCooldown()
        {
            StopCoroutine("cooldownCountdown"); // just to prevent double courutine at the same time
            timeOnCd = assignedSpell.cooldown;
            StartCoroutine("cooldownCountdown");
        }

        IEnumerator cooldownCountdown()
        {
            while(timeOnCd > 1)
            {
                timeOnCd--;
                cooldownTimer.text = timeOnCd.ToString();
                yield return new WaitForSeconds(1);
            }

            timeOnCd = 0.0f;
            cooldownTimer.text = "";
        }


    }
}