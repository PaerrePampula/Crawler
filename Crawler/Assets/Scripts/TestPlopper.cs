using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlopper : MonoBehaviour, IPlayerInteractable
{
    public delegate void Transistion(Action actionAfterTransistion);
    public static event Transistion onTransistion;
    [SerializeField] NeighborType doorLocation;
    [SerializeField] Room otherRoom;
    [SerializeField] string interactionText = "Press [E] to open";
    [SerializeField] string lockedDoorText = "Locked! Clear room of enemies to open";
    bool locked = false;
    [SerializeField] Transform lockVisual;
    public void DoPlayerInteraction()
    {
        if (!locked)
        {
            //Declare an anonymous function and use it as the action parameter
            onTransistion?.Invoke(() => CurrentRoomManager.Singleton.setNewRoom(otherRoom, doorLocation));
        }


    }
    public void SetLockState(bool state)
    {
        locked = state;
        if (lockVisual != null)
        {
            lockVisual.gameObject.SetActive(state);
        }
    }
    public string getPlayerInteractionString()
    {
        return (locked == false) ? interactionText : lockedDoorText;
    }


}
