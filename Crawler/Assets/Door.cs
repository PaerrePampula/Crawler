using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IPlayerInteractable
{
    public delegate void Transistion(Action actionAfterTransistion);
    public static event Transistion onTransistion;
    [SerializeField] NeighborType doorLocation;
    [SerializeField] string interactionText = "Press [E] to open";
    [SerializeField] string lockedDoorText = "Locked! Clear room of enemies to open";
    bool locked = false;
    [SerializeField] Transform lockVisual;
    Room room;
    // Start is called before the first frame update
    void Awake()
    {
        room = transform.root.GetComponent<Room>();
        room.AddRoomDoor(doorLocation, this.gameObject);
    }
    private void OnEnable()
    {
        if (room == null)
        {
            room = transform.root.GetComponent<Room>();
        }
        //Disable this door location if there is no connection available.
        if (room.getNeighbor(doorLocation) == null)
        {
            gameObject.SetActive(false);
        }
    }

    public void DoPlayerInteraction()
    {
        if (!locked)
        {
            //Declare an anonymous function and use it as the action parameter
            onTransistion?.Invoke(() => CurrentRoomManager.Singleton.setNewRoom(room.getNeighbor(doorLocation), doorLocation));
        }


    }
    public void SetLockState(bool state)
    {
        locked = state;
        if (lockVisual != null)
        {
            if (!locked)
            {
                lockVisual.GetComponent<Animator>().Play("Dissolve");
            }
        }
    }
    public string getPlayerInteractionString()
    {
        return (locked == false) ? interactionText : lockedDoorText;
    }
}
