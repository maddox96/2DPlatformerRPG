using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimatedPixelPack
{
    public class Toolbar : MonoBehaviour {

        private Text cooldownTimer;
        private float timeOnCd;
        public Image icon;

        public Spell assignedSpell;
        

        private void Awake()
        {
            cooldownTimer = transform.GetComponentInChildren<Text>();
            icon = transform.GetComponentInChildren<Image>();
        }

        void Start()
        {
            icon.sprite = assignedSpell.icon;
        }

        public bool IsOnCooldown // name to changeeeeeeee
        {
            get
            {
                if (timeOnCd <= 0)
                {
                    StopCoroutine("cooldownCountdown"); // just to prevent double courutine at the same time
                    timeOnCd = assignedSpell.cooldown;
                    StartCoroutine("cooldownCountdown");
                    return false;
                }
                else return true;
            }
        }
      
        IEnumerator cooldownCountdown()
        {
            while(timeOnCd > 0)
            {
                timeOnCd--;
                cooldownTimer.text = timeOnCd.ToString();
                yield return new WaitForSeconds(1);
            }
        }


    }
}