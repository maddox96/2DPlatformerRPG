using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AnimatedPixelPack
{
    public class EnemyManager : MonoBehaviour
    {

        public static EnemyManager EM;

        public Enemy testEnemy;
        public List<Enemy> Enemies;

        private void Start()
        {
            EM = this;
            Enemies = new List<Enemy>();
            SpawnEnemy(testEnemy);
            SpawnEnemy(testEnemy);
        }

        public void SpawnEnemy(Enemy enemy)
        {
            Enemy tempEnemy = Instantiate(enemy);
            Enemies.Add(tempEnemy);
        }

        public void KillEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }

}