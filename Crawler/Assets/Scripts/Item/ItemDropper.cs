using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Is a singleton in-scene that makes all the item drops on command
/// Handles all possible item drops
/// </summary>
class ItemDropper : MonoBehaviour
{
    static ItemDropper singleton;
    public static ItemDropper Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<ItemDropper>();
            }
            return singleton;
        }
    }
    [SerializeField] List<ItemScriptable> droppableItems = new List<ItemScriptable>();
    [SerializeField] GameObject genericPickup;
    private void OnEnable()
    {
        Room.onRoomClear += checkForRandomChanceToDropLootInRoom;
    }
    private void OnDisable()
    {
        Room.onRoomClear -= checkForRandomChanceToDropLootInRoom;
    }
    private void checkForRandomChanceToDropLootInRoom(Room room)
    {
        int random = UnityEngine.Random.Range(0, 101);
        if (random <= Globals.LikelinessOfItemDroppingInRoom)
        {
            GenerateRandomItemAtCurrentRoomDropPoint(room.PickupsDropPointOnRoomClear.position);
        }

    }

    public void GenerateRandomItemAtCurrentRoomDropPoint(Vector3 position)
    {
        int randomItemChoice = UnityEngine.Random.Range(0, droppableItems.Count);
        ItemScriptable itemScriptable = droppableItems[randomItemChoice];
        GameObject newPickup = Instantiate(genericPickup, position, Quaternion.identity, CurrentRoomManager.Singleton.currentRoom.gameObject.transform);
        newPickup.GetComponent<Pickup>().InitializePickUp(itemScriptable);
    }

}

