using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlopper : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] NeighborType doorLocation;
    [SerializeField] Room otherRoom;

    public void DoPlayerInteraction()
    {
        CurrentRoomManager.Singleton.setNewRoom(otherRoom, doorLocation);
    }

    public string getPlayerInteractionString()
    {
        return "Press [E] to open";
    }


}
