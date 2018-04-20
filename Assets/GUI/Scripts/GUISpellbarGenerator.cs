using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Portfolio
{
    public class GUISpellbarGenerator : GUIIconBarGenerator
    {

        Profession playerProfession;
        Spell[] spells;

        protected override IIconBarGeneratable[] GetObjsToGenerate()
        {
            return playerProfession.proffesionSpells;
        }

        protected override void OnIconInstantiate(GameObject createdIcon, int i)
        {
            Toolbar _temp;
            _temp = createdIcon.gameObject.AddComponent<Toolbar>();
            _temp.assignedSpell = spells[i];
            Spellbook.toolbars.Add(_temp);
        }

        protected override void Start()
        {
            playerProfession = new Wizard();
            spells = playerProfession.proffesionSpells;
            base.Start();       
        }     
    }
}