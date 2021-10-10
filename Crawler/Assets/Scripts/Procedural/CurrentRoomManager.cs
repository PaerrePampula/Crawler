using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls what room is active in scene. The room active is always the room where the player is
/// </summary>
public class CurrentRoomManager : MonoBehaviour
{
    //Save all possible combinations of doors as a dictionary to speed up lookup signifigantly (much faster than a switch case etc.)
    //there arent that many combinations, so i think there is no problem doing it this way.
    Dictionary<NeighborType, NeighborType> matchingDoorTypes = new Dictionary<NeighborType, NeighborType>
    {
        { NeighborType.Left, NeighborType.Right },
        { NeighborType.Right, NeighborType.Left },
        { NeighborType.Below, NeighborType.Above },
        { NeighborType.Above, NeighborType.Below },

    };
    public delegate void OnRoomSet(Room setRoom);
    public static event OnRoomSet onPlayerRoomSet;
    static CurrentRoomManager singleton;
    public static CurrentRoomManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<CurrentRoomManager>();
            }
            return singleton;
        }
    }
    private void OnEnable()
    {
        DungeonMap.onMapGenerationComplete += invokeInitialRoomInformation;
    }
    private void OnDestroy()
    {
        DungeonMap.onMapGenerationComplete -= invokeInitialRoomInformation;
    }
    private void invokeInitialRoomInformation()
    {
        //Player is spawned in currenly with just info in the inspector about current room
        onPlayerRoomSet?.Invoke(currentRoom);
    }
    public Transform GetCurrentRoomPickupPoint()
    {
        return currentRoom.PickupsDropPointOnRoomClear;
    }

    [SerializeField] public Room currentRoom;
    public void setNewRoom(Room nextRoom, NeighborType connectingNeighborDoor)
    {
        currentRoom.gameObject.SetActive(false);
        nextRoom.gameObject.SetActive(true);
        currentRoom = nextRoom;
        //Get the opposite neighbor type door to plop the player at the other room.
        NeighborType doorTypeToPlopPlayerTo = matchingDoorTypes[connectingNeighborDoor];
        currentRoom.WarpPlayerToDoorLocation(doorTypeToPlopPlayerTo);
        onPlayerRoomSet?.Invoke(currentRoom);
    }

}
