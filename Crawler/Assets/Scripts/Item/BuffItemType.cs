using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BuffItemType : IPlayerItemableBehaviourType
{
    public void DoItemPickupAction(Item item)
    {
        BuffItemScriptable buffItemScriptable = (BuffItemScriptable)item.ItemScriptable;
        Player.Singleton.GivePlayerItem(item);
        Player.Singleton.BuffStatModifier(buffItemScriptable.StatToBuff, buffItemScriptable.BuffAmountPercentage);
    }
}
