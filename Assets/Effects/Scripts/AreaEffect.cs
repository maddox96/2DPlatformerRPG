using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{


    public class AreaEffect : MonoBehaviour
    {


       
        public static List<Character> GetCharacters(Character caster, Vector3 position,float radius)
        {
            List<Character> characters = new List<Character>();
            Character _temp;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, radius);
            foreach(Collider2D c in hitColliders)
            {
                _temp = c.GetComponent<Character>();
                if (_temp != null && _temp != caster) characters.Add(_temp);
            }
            return characters;
        }

        public static void ApplyAreaDamage(List<Character> characters, int damage, Vector3 sourcePosition)
        {
            foreach (Character c in characters)
            {
                c.ApplyDamage(damage, sourcePosition.x - c.transform.position.x);
            }
        }

        public static void ApplyAreaEffect(List<Character> characters, EffectData effectData)
        {
            foreach (Character c in characters)
            {
                EffectManager.effectManager.ApplyEffect(c, effectData);
            }
        }

        public static void ApplyAreaEffect(List<Character> characters, Effects.BoolEffects effect, float duration)
        {
            foreach (Character c in characters)
            {
                EffectManager.effectManager.ApplyEffect(c, effect, duration);
            }
        }
    }
}