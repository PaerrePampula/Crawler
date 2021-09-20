using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName = "Item/Healing item", fileName = "New healing item")]
public class HealingItemScriptable : ItemScriptable
{
    [SerializeField] float healingAmount = 0.5f;

    public float HealingAmount { get => healingAmount; }
}
