
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ItemCreator
{
    public static Item CreateItemFromScriptable(ItemScriptable itemScriptable)
    {
        //Make an object of type item, find out what behaviour matches the item type of the scriptable
        //assign this behaviour to be the interface of the specific item and assign the scriptable to
        //be data of this new item.
        Item item = new Item();
        IPlayerItemableBehaviourType playerItemable = ItemBehaviourTypeInferringDatabase.inferItemBehaviourType(itemScriptable.ItemType);
        item.ItemScriptable = itemScriptable;
        item.Itemable = playerItemable;
        return item;
    }
    public static void CreateAndGiveItemToPlayer(ItemScriptable itemScriptable)
    {
        Item item = CreateItemFromScriptable(itemScriptable);
        item.DoItemPickupActionAccordingToItemType();
    }
}

