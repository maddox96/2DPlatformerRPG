using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Portfolio
{
    public class ReflectDamage : MonoBehaviour
    {
        // TODO: DETECT COLIISON AS OTHER CLASS THAN CHARACTER IN ORDER TO
        // COLLIDE ONLY WHEN ATTACKING 
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character _temp = collision.GetComponent<Character>();
            if(_temp != null)
            { 
                _temp.ApplyDamage(_temp.WeaponDamage);
            }
        }
    }
}