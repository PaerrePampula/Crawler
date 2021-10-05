using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class RevealMapItemType : IPlayerItemableBehaviourType
{
    public bool DoItemPickupAction(Item item)
    {
        if (item.ItemScriptable.PickupSound != null)
            Player.Singleton.GetComponent<AudioSource>().PlayOneShot(item.ItemScriptable.PickupSound);
        DungeonMap.Singleton.InvokeFullMapShow();
        return true;
    }
}
