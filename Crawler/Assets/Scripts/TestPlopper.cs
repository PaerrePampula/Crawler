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
    public void DoPlayerInteraction()
    {
        //Declare an anonymous function and use it as the action parameter
        onTransistion?.Invoke(() => CurrentRoomManager.Singleton.setNewRoom(otherRoom, doorLocation));

    }

    public string getPlayerInteractionString()
    {
        return interactionText;
    }


}
