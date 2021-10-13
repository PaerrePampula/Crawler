using System;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] ItemScriptable _itemScriptable;
    Item _item;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [SerializeField] GameObject collectEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupItem();
        }
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
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 250f);
        if (_itemScriptable != null)
        {
            InitializePickupBasedOnScriptable();
        }
    }
    protected void PickupItem()
    {
        //Item pickup returns pickup success
        //If item returns false == player cant pick up, do nothing
        if (_item.DoItemPickupActionAccordingToItemType())
        {
            PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay("I just received an item: " + _item.ItemScriptable.ItemName);
            Instantiate(collectEffect, transform.position += Vector3.up * 0.5f, transform.rotation = Quaternion.Euler(30, 0, 0));
            Destroy(gameObject);
        }
        else
        {
            PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay("There is an item on the ground:"  + _item.ItemScriptable.ItemName + ", I cant pick it up, im full on HP");
        }
        


    }
}

