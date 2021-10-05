using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatBuffTrackerUI : MonoBehaviour
{
    [SerializeField] StatTypeToSpriteAsset statTypesToSprites;
    Dictionary<StatType, Sprite> statsToSprites = new Dictionary<StatType, Sprite>();
    Dictionary<StatType, GameObject> currentBuffs = new Dictionary<StatType, GameObject>();
    [SerializeField] GameObject genericSpriteForStatBuffs;
    private void Awake()
    {
        //Initialize dictionary of sprites for stat types using a scriptableobject asset
        for (int i = 0; i < statTypesToSprites.statTypesToSprites.Count; i++)
        {
            statsToSprites[statTypesToSprites.statTypesToSprites[i].ItemBehaviourType] =
                statTypesToSprites.statTypesToSprites[i].Sprite;
        }
    }
    private void OnEnable()
    {
        Player.onPlayerReceivedItem += updateStatBuffsIfNeeded;
    }
    private void OnDisable()
    {
        Player.onPlayerReceivedItem -= updateStatBuffsIfNeeded;
    }
    private void updateStatBuffsIfNeeded(Item item)
    {
        if (item.ItemScriptable.ItemType == ItemBehaviourType.BuffPlayerStats)
        {
            AddNewBuff(item);
        }
    }

    private void AddNewBuff(Item item)
    {
        BuffItemScriptable buffItem = (BuffItemScriptable)item.ItemScriptable;
        if (buffItem.StatToBuff == StatType.MaxHP) return;
        if (currentBuffs.ContainsKey(buffItem.StatToBuff))
        {
            currentBuffs[buffItem.StatToBuff].GetComponent<StatBuffUI>().ChangePercentage(buffItem.BuffAmountPercentage);
        }
        else
        {
            GameObject go = Instantiate(genericSpriteForStatBuffs, transform);
            currentBuffs[buffItem.StatToBuff] = go;
            go.GetComponent<StatBuffUI>().Initialize(statsToSprites[buffItem.StatToBuff]);
            go.GetComponent<StatBuffUI>().ChangePercentage(buffItem.BuffAmountPercentage);
        }
    }
}
