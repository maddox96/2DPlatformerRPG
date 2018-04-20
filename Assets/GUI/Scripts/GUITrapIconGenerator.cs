using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Portfolio
{
    public class GUITrapIconGenerator : GUIIconBarGenerator
    {
        Profession playerProfession;
        Trap[] traps;

        protected override IIconBarGeneratable[] GetObjsToGenerate()
        {
            return traps;
        }

        protected override void OnIconInstantiate(GameObject createdIcon, int i)
        {
            createdIcon.AddComponent<BuyButton>().holdedObject = traps[i];
        }

        protected override void Start()
        {
            playerProfession = new Wizard();
            traps = playerProfession.proffesionTraps;
            base.Start();
        }

       
    }
}