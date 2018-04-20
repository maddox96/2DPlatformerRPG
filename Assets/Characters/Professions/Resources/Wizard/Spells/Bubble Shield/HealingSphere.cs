using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class HealingSphere : MonoBehaviour {


        int objsToHealSize = 25, objsInSphere;

        public int healthPerTick;
        public float timeBeetwenTicks;
        public string healTag = "Player";
        Collider2D col;
        bool readyToHeal = true;
        Collider2D[] objsToHeal;
        Character objToHeal;
        float currentTime;

        private void Start()
        {
            objsToHeal = new Collider2D[objsToHealSize];

            col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void Update()
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0.0f)
            {
                currentTime = timeBeetwenTicks;
                readyToHeal = true;
                Heal();
            }
        }

        void Heal()
        {
            Debug.Log("healing");
            objsInSphere = col.GetContacts(objsToHeal);
            for(int i = 0; i < objsInSphere; i++)
            {
                if(objsToHeal[i].tag == healTag)
                {
                    objToHeal = objsToHeal[i].GetComponent<Character>();
                    objToHeal.Heal(healthPerTick);
                    readyToHeal = false;
                }
            }
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == healTag && readyToHeal) Heal();
        }
    }
}