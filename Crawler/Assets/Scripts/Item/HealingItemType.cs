using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class HealingItemType : IPlayerItemableBehaviourType
{
    //Pass in the item as argument, find the scriptable and act according to data on SO
    public void DoItemPickupAction(Item item)
    {
        //Typecast the scriptable to correct type to use the info found on scriptable correctly
        HealingItemScriptable healingItemScriptable = (HealingItemScriptable)item.ItemScriptable;
        if (item.ItemScriptable.PickupSound != null)
            Player.Singleton.GetComponent<AudioSource>().PlayOneShot(item.ItemScriptable.PickupSound);
        Player.Singleton.ChangeHp(healingItemScriptable.HealingAmount);
    }
}
