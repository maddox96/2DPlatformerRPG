using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{

    public class OCEDealDMG : MonoBehaviour {

        Quaternion rotation;
        ParticleSystem part;
        List <ParticleCollisionEvent> collisionEvents;
        private void Start()
        {
            rotation = transform.rotation;
            collisionEvents = new List<ParticleCollisionEvent>();
            part = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            transform.rotation = rotation;
        }
      
        private void OnParticleCollision(GameObject other)
        {
                //int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        }    
    }
}