using System;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] ItemScriptable _itemScriptable;
    Item _item;
    [SerializeField] SpriteRenderer _spriteRenderer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        PickupItem();
    }
    public void InitializePickUp(ItemScriptable itemScriptable)
    {
        _itemScriptable = itemScriptable;
        InitializePickupBasedOnScriptable();
    }

    private void InitializePickupBasedOnScriptable()
    {
        _item = ItemCreator.CreateItemFromScriptable(_itemScriptable);
        _spriteRenderer.sprite = _itemScriptable.ItemSprite;
        gameObject.name = _itemScriptable.name + " pickup";
    }

    private void OnEnable()
    {
        if (_itemScriptable != null)
        {
            InitializePickupBasedOnScriptable();
        }
    }
    protected void PickupItem()
    {
        _item.DoItemPickupActionAccordingToItemType();
        PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay("I just received an item: " + _item.ItemScriptable.ItemName);
        Destroy(gameObject);

    }
}

