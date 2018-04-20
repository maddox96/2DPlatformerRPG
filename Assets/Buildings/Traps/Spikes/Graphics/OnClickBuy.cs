using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portfolio
{
    public class OnClickBuy : OnClickHolder<IBuyable> {

        protected override void OnClickEvent()
        {
            holdedObject.Buy();
        }

        void hideButton()
        {
            if (!GameManager.manager.player.enoughMoney(holdedObject))
            {
                if (button != null)
                    button.interactable = false;
            }
        }

        private void OnDestroy()
        {
            GameManager.manager.player.OnMoneySpend -= hideButton;
        }

        protected override void Start()
        {
            base.Start();
            GameManager.manager.player.OnMoneySpend += hideButton;
            hideButton();
        }
    }
}