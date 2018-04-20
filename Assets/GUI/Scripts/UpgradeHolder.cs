using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace Portfolio
{
    public class UpgradeHolder : OnClickHolder<Upgrade>
    {
        
        void hideButton()
        {
            if (!GameManager.manager.player.enoughMoney(holdedObject))
            {
                if(button != null)
                    button.interactable = false;
            }
        }

        protected override void Start()
        {
            base.Start();
            GameManager.manager.player.OnMoneySpend += hideButton;
            hideButton();
        }

        protected override void OnClickEvent()
        {
            holdedObject.Buy();
        }
    }
}