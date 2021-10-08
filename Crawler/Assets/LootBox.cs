using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] List<ItemScriptable> possibleLoot = new List<ItemScriptable>();
    [SerializeField] InputAlias[] interactions;
    Animator _animator;
    bool opened = false;
    [SerializeField] StateOnAnimationTrigger trigger;
    [SerializeField] Transform dropPoint;
    public void DoPlayerInteraction()

    {
        if (!opened) _animator.Play("LootChestOpen");
        opened = true;
    }

    public bool getPlayerInteraction()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public InputAlias[] getPlayerInteractions()
    {
        return interactions;
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

    }
    private void OnEnable()
    {
        trigger.onTriggerState += spawnItem;
    }
    private void OnDisable()
    {
        trigger.onTriggerState -= spawnItem;
    }

    private void spawnItem()
    {
        int randomLootFromList = UnityEngine.Random.Range(0, possibleLoot.Count);
        ItemScriptable item = possibleLoot[randomLootFromList];
        ItemDropper.Singleton.GenerateChosenItemAtDropPoint(dropPoint.position, item);
    }
}
