using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public abstract class Profession
    {

        public abstract string proffesionName { get; }
        public Spell[] proffesionSpells;
        public Trap[] proffesionTraps;

        public Profession()
        {
            LoadAndAssignProffesionTraps();
            LoadAndAssignProffesionSpells();
        }

        protected void LoadAndAssignProffesionSpells()
        {
            string path = proffesionName + "\\Spells";
            proffesionSpells = Resources.LoadAll<Spell>(path);
        }

        protected void LoadAndAssignProffesionTraps()
        {
            string path = "Traps";
            proffesionTraps = Resources.LoadAll<Trap>(path);
        }
    }
}