using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// All items are considered to be just items in technicality, but they are assigned an IPlayerItemableBehaviourType
/// interface, which actually contains information about what action is taken during item pickup
/// This can be and is not limited to
/// Healing type of item, consumed on pickup
/// Buff type of item, will buff player based on enum deciding what gets buffed
/// Gold type of item. adds money to player
/// </summary>
public class Item
{
    ItemScriptable _itemScriptable;
    public ItemScriptable ItemScriptable { get => _itemScriptable; set => _itemScriptable = value; }
    public IPlayerItemableBehaviourType Itemable { get => itemable; set => itemable = value; }
    IPlayerItemableBehaviourType itemable;
    int _itemCount = 1;

    public int ItemCount { get => _itemCount; set => _itemCount = value; }
    public bool DoItemPickupActionAccordingToItemType()
    {
        //Pass in this item class to do action according to the itemscriptable, which can be any 
        //Scriptable of type itemscriptable on the base level
        return itemable.DoItemPickupAction(this);
    }
}