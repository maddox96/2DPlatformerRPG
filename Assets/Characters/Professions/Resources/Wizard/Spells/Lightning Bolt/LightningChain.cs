using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{
    public class LightningChain : MonoBehaviour
    {
        [SerializeField]
        LightningChain lightningChain;
        LightningStrike lightningStrike;
        LineRenderer lineRenderer;

        int controlPoints;
        Vector3[] points;
        Character farthestCharacter;

       private void OnDrawGizmos()
       {
           Gizmos.DrawSphere(transform.position, 3.0f);
       }


        void Start()
        {
            Invoke("DestroyAndSpawn", 0.05f);
            lineRenderer = GetComponent<LineRenderer>();
            lightningStrike = GetComponentInParent<LightningStrike>();

            controlPoints = Random.Range(1, 4);
            points = new Vector3[controlPoints + 2];
            farthestCharacter = GetFarthestCharacter();

            if (!farthestCharacter)
            {
                Destroy(gameObject);
                return;
            }

            SetPoints();
         }
        

        void DestroyAndSpawn()
        {
            Destroy(gameObject, 0.1f);
            Instantiate(lightningChain, farthestCharacter.transform.position, Quaternion.identity, transform.parent.transform);
        }

        Character GetFarthestCharacter()
        {
            Character farthestCharacter = null;
            float distance = 0.0f;
            List<Character> _temp = AreaEffect.GetCharacters(null, transform.position, 3.0f);

            foreach (Character c in _temp)
            {
                if (!lightningStrike.characterHitted.Add(c)) continue;
                if (Vector2.Distance(transform.position, c.transform.position) > distance)
                {
                    farthestCharacter = c;
                    distance = Vector2.Distance(transform.position, c.transform.position);
                }
            }

            return farthestCharacter;
        }

        void SetPoints()
        {
            points[0] = transform.position;
            float xDistance = Mathf.Abs(transform.position.x - farthestCharacter.transform.position.x);
            xDistance /= controlPoints;
            float randomX, randomY;

            for (int i = 1; i < controlPoints + 2; i++)
            {
                randomX = Random.Range(points[i - 1].x, farthestCharacter.transform.position.x);
                randomY = Random.Range(transform.position.y - 1.0f, transform.position.y + 1.0f);
                points[i] = new Vector3(randomX, randomY, -1.0f);
            }

            points[controlPoints + 1] = farthestCharacter.transform.position;
            lineRenderer.positionCount = controlPoints + 2;
            lineRenderer.SetPositions(points);
        }
    }
}