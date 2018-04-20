using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Portfolio
{
    public class SpellsUpgradeGUI : MonoBehaviour
    {
        public static SpellsUpgradeGUI UpgradeGUI;

        [SerializeField]
         GameObject spellHolderPrefab;
        [SerializeField]
         GameObject upgradeHolderPrefab;

        [SerializeField]
         Transform gridParent;
        [SerializeField]
         Transform upgradeGridParent;

        private void Awake()
        {
            UpgradeGUI = this;
            GenerateSpellGUI(new Wizard());
        }

        void GenerateSpellGUI(Profession w)
        {
            foreach (Spell spell in w.proffesionSpells)
            {
                GameObject go = Instantiate(spellHolderPrefab, gridParent);

                go.GetComponent<SpellHolder>().holdedObject = spell;

                Image[] _temp = go.GetComponentsInChildren<Image>();
                for (int i = 0; i < _temp.Length; i++)
                {
                    if (_temp[i].transform != go.transform)
                        _temp[i].sprite = spell.spellBookIcon;
                }

                Text[] _temp2 = go.GetComponentsInChildren<Text>();
                for (int i = 0; i < _temp2.Length; i++)
                {
                    if (_temp2[i].transform != go.transform)
                        _temp2[i].text = spell.spellName;
                }
            }
        }

        public void GenerateUpdatesGUI(Spell spell)
        {
            Upgrade[] upgrades = spell.GetSpellUpgrades();
            int initedHolders = upgradeGridParent.childCount;
          
            for (int i = 0; i < initedHolders; i++)
            {
                Destroy(upgradeGridParent.GetChild(i).gameObject); 
            }

            foreach(Upgrade u in upgrades)
            {
                GameObject go = Instantiate(upgradeHolderPrefab, upgradeGridParent);
                Text[] texts = go.GetComponentsInChildren<Text>();
                foreach(Text text in texts)
                {
                    if (text.transform.name == "Name")
                        text.text = u.name;
                    else if (text.transform.name == "Description")
                        text.text = u.description;
                    else if (text.transform.name == "Cost")
                        text.text = "COST\n" + u.cost.ToString();
                }
                go.GetComponent<BuyButton>().holdedObject = u;
            }
        }
    }

}