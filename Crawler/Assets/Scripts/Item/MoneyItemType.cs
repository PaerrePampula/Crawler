using System.Collections;
using UnityEngine;


public class MoneyItemType : IPlayerItemableBehaviourType
{
    public void DoItemPickupAction(Item item)
    {
        MoneyItemScriptable moneyItemScriptable = (MoneyItemScriptable)item.ItemScriptable;
        Player.Singleton.GivePlayerItem(item);
        if (item.ItemScriptable.PickupSound != null)
            Player.Singleton.GetComponent<AudioSource>().PlayOneShot(item.ItemScriptable.PickupSound);
        PlayerEconomy.Singleton.ChangePlayerMoney(moneyItemScriptable.MoneyAmount);
    }

}
