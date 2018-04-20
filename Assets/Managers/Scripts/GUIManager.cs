using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Portfolio
{

    public class GUIManager : MonoBehaviour {

        public static GUIManager manager;


        public GUIIconBarGenerator Spellbar;
        public GUITrapIconGenerator Trapbar;

        private void Awake()
        {
            manager = this;
        }

        Image _activeToolbar;

        void GUIGenerate(IGUIGeneratable GUIElement)
        {
            GUIElement.Generate();
        }

         void HideGUIElement(GameObject g)
        {
            g.SetActive(false);
        }

        public void HideTrapbar()
        {
            HideGUIElement(Trapbar.gameObject);
        }

        public void GenerateTrapbar()
        {
            GUIGenerate(Trapbar);
        }

        public void GenerateSpellbar()
        {
            GUIGenerate(Spellbar);
        }


        public Image activeToolbar
        {
            get { return _activeToolbar; }

            set
            {
                if (_activeToolbar != null) activeToolbar.color = Color.white;
                _activeToolbar = value;
                _activeToolbar.color = Color.grey;
            }
        }
    }
}