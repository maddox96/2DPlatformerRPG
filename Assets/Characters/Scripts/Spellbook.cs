using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Portfolio
{

    [RequireComponent(typeof(Character))]
    public class Spellbook : MonoBehaviour
    {
        
        [Header("TOOLBARS PREFABS")]
        public Transform toolbarsParent;
        public Toolbar toolbarPrefab;
        public Sprite toolbarConnector;
        public Sprite toolbarEnd;
        public Image imagePrefab;
        [Header("LIST OF CHARACTER SPELLS")]
        public List<Spell> spells = new List<Spell>();

        public static List<Toolbar> toolbars = new List<Toolbar>();

        [HideInInspector]
        public Toolbar currentToolbar;
      
        public Spell currentSpell;

   
        private Character caster;

        private void Start()
        {
            caster = GetComponent<Character>();
            //SetToolbar(1);
        }

       

        public void SetToolbar(int pressedKey)
        {
            if (caster.isCasting)
                caster.CancelCasting();

            currentSpell = toolbars[pressedKey - 1].assignedSpell;  
            currentToolbar = toolbars[pressedKey - 1];
            GUIManager.manager.activeToolbar = currentToolbar.icon;
        }

      
        public Spell Cast(float castTime = 1)
        {

            Spell _temp = null;
            if (caster.enoughMana(currentSpell) && !currentToolbar.IsOnCooldown)
            {
                caster.DrainMana(currentSpell.manaCost);
                currentToolbar.SetCooldown();                
                _temp = currentSpell.Cast(caster);
                _temp.castedTime = castTime;
            }

            return _temp;
        }
    }
}
