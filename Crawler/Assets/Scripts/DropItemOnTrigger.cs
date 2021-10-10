using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemOnTrigger : MonoBehaviour
{
    [SerializeField] List<ItemScriptable> itemdrops;
    public void TryTriggerDrop()
    {
        float randomChance = Random.Range(0f, 100f);
        if (randomChance <= Globals.LikelinessOfContainerDroppingHp) DropItem();
    }

    private void DropItem()
    {
        int randomItemChoice = Random.Range(0, itemdrops.Count);
        ItemDropper.Singleton.GenerateChosenItemAtDropPoint(transform.position + Vector3.up*0.5f, itemdrops[randomItemChoice]);
    }
}
