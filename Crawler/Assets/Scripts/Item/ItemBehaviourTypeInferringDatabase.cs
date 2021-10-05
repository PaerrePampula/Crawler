using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ItemBehaviourTypeInferringDatabase
{
    static Dictionary<ItemBehaviourType, IPlayerItemableBehaviourType> itemBehaviourTypeDatabase = new Dictionary<ItemBehaviourType, IPlayerItemableBehaviourType>
    {
        { ItemBehaviourType.Healing, new HealingItemType() },
        { ItemBehaviourType.BuffPlayerStats, new BuffItemType() },
        { ItemBehaviourType.Money, new MoneyItemType() },
        { ItemBehaviourType.MapReveal, new RevealMapItemType() }
    };
    public static IPlayerItemableBehaviourType inferItemBehaviourType(ItemBehaviourType itemType)
    {
        return itemBehaviourTypeDatabase[itemType];
    }
};

public enum ItemBehaviourType
{
    Healing,
    BuffPlayerStats,
    Money,
    MapReveal
}