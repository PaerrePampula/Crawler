using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class BuffItemType : IPlayerItemableBehaviourType
{
    public bool DoItemPickupAction(Item item)
    {
        BuffItemScriptable buffItemScriptable = (BuffItemScriptable)item.ItemScriptable;
        Player.Singleton.GivePlayerItem(item);
        if (item.ItemScriptable.PickupSound != null)
            Player.Singleton.GetComponent<AudioSource>().PlayOneShot(item.ItemScriptable.PickupSound);
        Player.Singleton.BuffStatModifier(buffItemScriptable.StatToBuff, buffItemScriptable.BuffAmountPercentage);
        //Can always pickup buffs
        return true;
    }
}
