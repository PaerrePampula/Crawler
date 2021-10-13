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
    [SerializeField] List<HPItemAndSpawnChance> hPItemAndSpawnChances = new List<HPItemAndSpawnChance>();

    private void OnEnable()
    {
        SubscribeLootDrop();
        BaseMook.onMookPossibleItemDrop += GenerateRandomHealthItemAtMookDeath;
    }

    public void SubscribeLootDrop()
    {
        Room.onRoomClear += checkForRandomChanceToDropLootInRoom;
    }

    private void OnDisable()
    {
        UnSubscribeLootDrop();
        BaseMook.onMookPossibleItemDrop -= GenerateRandomHealthItemAtMookDeath;
    }

    public void UnSubscribeLootDrop()
    {
        Room.onRoomClear -= checkForRandomChanceToDropLootInRoom;
    }

    private void checkForRandomChanceToDropLootInRoom(Room room)
    {
        int random = UnityEngine.Random.Range(0, 101);
        if (random <= Globals.LikelinessOfItemDroppingInRoom)
        {
            GenerateRandomItemAtDropPoint(room.PickupsDropPointOnRoomClear.position);
        }

    }
    private void Start()
    {
        SortMookDrops();
    }
    public void GenerateChosenItemAtDropPoint(Vector3 position, ItemScriptable itemScriptable)
    {
        GameObject newPickup = Instantiate(genericPickup, position, Quaternion.identity, CurrentRoomManager.Singleton.currentRoom.gameObject.transform);
        newPickup.GetComponent<Pickup>().InitializePickUp(itemScriptable);
    }
    public void GenerateRandomItemAtDropPoint(Vector3 position)
    {
        int randomItemChoice = UnityEngine.Random.Range(0, droppableItems.Count);
        ItemScriptable itemScriptable = droppableItems[randomItemChoice];
        GameObject newPickup = Instantiate(genericPickup, position, Quaternion.identity, CurrentRoomManager.Singleton.currentRoom.gameObject.transform);
        newPickup.GetComponent<Pickup>().InitializePickUp(itemScriptable);
    }
    void GenerateRandomHealthItemAtMookDeath(Vector3 position)
    {
        float randomChance = UnityEngine.Random.Range(0, 101);


        if (randomChance <= Globals.LikelinessOfMookDroppingHp)
        {
            int chosenIndex = 0;
            //Passed likeliness check!
            float randomChanceForSpecificHealthItem = UnityEngine.Random.Range(0, 1f);
            //Binary sort the list.
            //initially the min numer will be the lowest index, the highest number will be the last index
            //an index will be taken from the middle, if the middle index member has a min value bigger than the value generated, the new max will be smaller than the current "middle" index
            //meaning that the list will be half the size. the new min is now the middle of these values... this is iterated until the value wanted is found
            int minNumber = 0;
            int maxNumber = hPItemAndSpawnChances.Count - 1;

            while (minNumber<= maxNumber)
            {
                int middle = (minNumber + maxNumber) / 2;
                if (hPItemAndSpawnChances[middle].MinimumValue < randomChanceForSpecificHealthItem && hPItemAndSpawnChances[middle].MaximumValue > randomChanceForSpecificHealthItem)
                {
                    chosenIndex = middle;
                    break;
                }
                else if (hPItemAndSpawnChances[middle].MinimumValue > randomChanceForSpecificHealthItem) maxNumber = middle - 1;
                else
                {
                    minNumber = middle + 1;
                }
            }
            ItemScriptable itemScriptable = hPItemAndSpawnChances[chosenIndex].HpItemDroppedByMook;
            GameObject newPickup = Instantiate(genericPickup, position, Quaternion.identity, CurrentRoomManager.Singleton.currentRoom.gameObject.transform);
            newPickup.GetComponent<Pickup>().InitializePickUp(itemScriptable);
        }

    }
    void SortMookDrops()
    {
        float sumOfWeights = 0;
        float normalizingValue = 1;
        for (int i = 0; i < hPItemAndSpawnChances.Count; i++)
        {
            sumOfWeights += hPItemAndSpawnChances[i].Weight;
            hPItemAndSpawnChances[i].MaximumValue = hPItemAndSpawnChances[i].Weight;
        }
        normalizingValue = normalizingValue / sumOfWeights;
        for (int i = 0; i < hPItemAndSpawnChances.Count; i++)
        {
            hPItemAndSpawnChances[i].MinimumValue = (i == 0) ? 0 : hPItemAndSpawnChances[i - 1].MaximumValue;
            hPItemAndSpawnChances[i].MaximumValue = hPItemAndSpawnChances[i].MinimumValue + hPItemAndSpawnChances[i].MaximumValue * normalizingValue;
        }

    }

}
[Serializable]
class HPItemAndSpawnChance
{
    [SerializeField] ItemScriptable hpItemDroppedByMook;
    [SerializeField] float weight;
    float minimumValue;
    float maximumValue;

    public float MinimumValue { get => minimumValue; set => minimumValue = value; }
    public float MaximumValue { get => maximumValue; set => maximumValue = value; }
    public float Weight { get => weight; set => weight = value; }
    public ItemScriptable HpItemDroppedByMook { get => hpItemDroppedByMook; set => hpItemDroppedByMook = value; }
}

