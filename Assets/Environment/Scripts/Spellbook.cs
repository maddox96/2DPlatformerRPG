using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AnimatedPixelPack
{

    [RequireComponent(typeof(Character))]
    public class Spellbook : MonoBehaviour
    {
        
        [Header("TOOLBARS PREFABS")]
        public Transform toolbarsParent;
        public Toolbar toolbarPrefab;

        [Header("LIST OF CHARACTER SPELLS")]
        public List<Spell> spells = new List<Spell>();

        private List<Toolbar> toolbars = new List<Toolbar>();

        [HideInInspector]
        public Toolbar currentToolbar;
        [HideInInspector]
        public Spell currentSpell;

        private void Start()
        {
            Init();
            SetToolbar(1);
        }

        void Init()
        {
            foreach(Spell spell in spells)
            {
                Toolbar tb = Instantiate(toolbarPrefab);
                tb.transform.SetParent(toolbarsParent, false);
                tb.GetComponent<RectTransform>().localPosition = new Vector3(toolbars.Count * 100.0f, -50.0f, 0.0f);
                tb.assignedSpell = spell;
                toolbars.Add(tb);
            }
        }

        public void SetToolbar(int pressedKey)
        {
            currentSpell = toolbars[pressedKey - 1].assignedSpell;
            currentToolbar = toolbars[pressedKey - 1];
            GUIManager.GUI.activeToolbar = currentToolbar.icon;
        }

        public void Cast(Character caster)
        {
            if(ApplyManaCost(currentSpell, caster) && !currentToolbar.IsOnCooldown)
                currentSpell.Cast(currentSpell, caster);
        }

        bool ApplyManaCost(Spell s, Character caster)
        {
            if (caster.CurrentMana - s.manaCost >= 0)
            {
                caster.DrainMana(currentSpell.manaCost);
                return true;
            }

            return false;
        }
    }
}
