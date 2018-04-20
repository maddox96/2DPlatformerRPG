using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{


    public class Spawner : MonoBehaviour
    {
        public GameObject objToSpawn;
        public float timeBeetwenSpawn, startDelay;
        public bool spawnAsChild;
        public bool isSpawning = true;
        public Vector2 spawnRandomRange;

        private float currentTime;
        private GameObject spawnedObject;
        private float randomX, randomY;

        void Start()
        {
            currentTime = startDelay;
        }

        void Update()
        {
            if (!isSpawning) return;

            currentTime -= Time.deltaTime;

            if (currentTime <= 0.0f)
            {
                Spawn();
            }
        }

        void Spawn()
        {
            currentTime = timeBeetwenSpawn;

            randomX = Random.Range(-spawnRandomRange.x, spawnRandomRange.x);
            randomY = Random.Range(-spawnRandomRange.y, spawnRandomRange.y);
            Vector3 target = new Vector3(transform.position.x + randomX, transform.position.y + randomY, 0.0f);

            Spell _spell = objToSpawn.GetComponent<Spell>();
            if(_spell)
            {
                _spell = _spell.Cast(target);
                _spell.transform.SetParent(transform, true);
                return;
            }

            spawnedObject = Instantiate(objToSpawn, target, Quaternion.identity);
            if (spawnAsChild) spawnedObject.transform.SetParent(transform, true);
        }
    }
}