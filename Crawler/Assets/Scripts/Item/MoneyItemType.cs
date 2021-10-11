using System.Collections;
using UnityEngine;


public class MoneyItemType : IPlayerItemableBehaviourType
{
    public delegate void MoneyChange(int amount);
    public static event MoneyChange onMoneyChange;
    public bool DoItemPickupAction(Item item)
    {
        MoneyItemScriptable moneyItemScriptable = (MoneyItemScriptable)item.ItemScriptable;
        Player.Singleton.GivePlayerItem(item);
        if (item.ItemScriptable.PickupSound != null)
            Player.Singleton.GetComponent<AudioSource>().PlayOneShot(item.ItemScriptable.PickupSound);
        PlayerEconomy.Singleton.ChangePlayerMoney(moneyItemScriptable.MoneyAmount);
        onMoneyChange?.Invoke(moneyItemScriptable.MoneyAmount);
        //Can always pickup money
        return true;
    }

}
