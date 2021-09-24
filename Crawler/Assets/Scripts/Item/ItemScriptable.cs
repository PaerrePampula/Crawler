using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ItemScriptable : ScriptableObject
{
    [SerializeField] string itemID;
    [SerializeField] string itemName;
    [TextArea]
    [SerializeField] string itemDescription;
    [SerializeField] ItemBehaviourType itemType;
    [SerializeField] Sprite itemSprite;
    [SerializeField] AudioClip pickupSound;
    //These fields definitely never should be also setters, because that would lead to data corruption of scriptableobjects
    public ItemBehaviourType ItemType { get => itemType;}
    public Sprite ItemSprite { get => itemSprite; }
    public string ItemDescription { get => itemDescription; }
    public string ItemName { get => itemName; }
    public string ItemID { get => itemID; }
    public AudioClip PickupSound { get => pickupSound; set => pickupSound = value; }
}
