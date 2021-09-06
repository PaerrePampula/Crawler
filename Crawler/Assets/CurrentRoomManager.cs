using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomManager : MonoBehaviour
{
    //Save all possible combinations of doors as a dictionary to speed up lookup signifigantly (much faster than a switch case etc.)
    //there arent that many combinations, so i think there is no problem doing it this way.
    Dictionary<NeighborType, NeighborType> matchingNeighborTypes = new Dictionary<NeighborType, NeighborType>
    {
        { NeighborType.Left, NeighborType.Right },
        { NeighborType.Right, NeighborType.Left },
        { NeighborType.Below, NeighborType.Above },
        { NeighborType.Above, NeighborType.Below },
        { NeighborType.LeftTop, NeighborType.RightTop },
        { NeighborType.RightTop, NeighborType.LeftTop },
        { NeighborType.AboveRight, NeighborType.BelowRight },
        { NeighborType.BelowRight, NeighborType.AboveRight }
    };
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

    [SerializeField] public Room currentRoom;
    public void setNewRoom(Room nextRoom, NeighborType connectingNeighborDoor)
    {
        currentRoom.gameObject.SetActive(false);
        nextRoom.gameObject.SetActive(true);
        currentRoom = nextRoom;
        //Get the opposite neighbor type door to plop the player at the other room.
        NeighborType doorTypeToPlopPlayerTo = matchingNeighborTypes[connectingNeighborDoor];
        currentRoom.WarpPlayerToDoorLocation(doorTypeToPlopPlayerTo);
    }

}
