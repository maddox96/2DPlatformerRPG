using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Portfolio
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
        }

        public void SpawnEnemy()
        {
            Enemy tempEnemy = Instantiate(testEnemy);
            Enemies.Add(tempEnemy);
        }

        public void KillEnemy(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }

}