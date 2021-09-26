using System.Collections;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/Money item", fileName = "New money item")]
public class MoneyItemScriptable : ItemScriptable
{
    public int moneyAmount;
    public int MoneyAmount { get => moneyAmount; }
}
