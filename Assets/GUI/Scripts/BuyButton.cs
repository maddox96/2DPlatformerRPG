
namespace Portfolio
{
    public class BuyButton : OnClickHolder<IBuyable>
    {
        protected override void Start()
        {
            base.Start();
            GameManager.manager.player.OnMoneySpend += hideButton;
            hideButton();
        }

        protected override void OnClickEvent()
        {
            if(GameManager.manager.player.enoughMoney(holdedObject))
            {
                holdedObject.Buy();
                GameManager.manager.player.spendMoney(holdedObject);
            }                
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
    }


}
