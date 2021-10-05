using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Buff item", fileName = "New buff item")]
public class BuffItemScriptable : ItemScriptable
{
    [SerializeField] StatType statToBuff;
    [SerializeField] float buffAmountPercentage;

    public StatType StatToBuff { get => statToBuff; }
    public float BuffAmountPercentage { get => buffAmountPercentage; }
}
public enum StatType
{
    Damage,
    Hp,
    MaxHP,
    AttackSpeed,
    MovementSpeed,
    Armor,
    ItemDiscovery,
    CritChance,
    PartialDamageReductionChance,
    OnKillBomb
}