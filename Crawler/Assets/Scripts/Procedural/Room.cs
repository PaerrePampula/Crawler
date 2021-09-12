
using System;
using System.Collections.Generic;
using UnityEngine;
//Used by all prefabs marked as rooms, rooms should have (currently) a size
//of either 1 or 2. 1 size room will be depicted as a 1x1 room on the map
//2 room will be depicted as 2x2 size on the map

public class Room : MonoBehaviour
{
    Dictionary<NeighborType, Room> roomNeighbors = new Dictionary<NeighborType, Room>();
    Dictionary<NeighborType, GameObject> roomDoors = new Dictionary<NeighborType, GameObject>();
    [SerializeField] List<DoorLocation> doorLocations = new List<DoorLocation>();
    [SerializeField] List<RoomNeighbor> roomNeighborsList = new List<RoomNeighbor>();
    [SerializeField] int _cellSize;
    GameObject roomPrefab;
    [SerializeField] Transform enemyContainer;
    List<BaseMook> roomMooks = new List<BaseMook>();
    int roomMookCount;
    Cell _cell;
    public void AddRoomDoor(NeighborType neighborType, GameObject door)
    {
        roomDoors.Add(neighborType, door);
    }
    private void Start()
    {
        if (CurrentRoomManager.Singleton != null)
        {
            if (this != CurrentRoomManager.Singleton.currentRoom) gameObject.SetActive(false);
        }

        SaveListsToDictionaries();
        SetInfoAboutRoomMooks();
    }

    private void SaveListsToDictionaries()
    {
        //Save the list as a dictionary to make lookups a bit better
        for (int i = 0; i < roomNeighborsList.Count; i++)
        {
            roomNeighbors.Add(roomNeighborsList[i].NeighborType, roomNeighborsList[i].NeighborRoom);
        }
        for (int i = 0; i < doorLocations.Count; i++)
        {
            roomDoors.Add(doorLocations[i].NeighborType, doorLocations[i].Location);
        }
    }

    private void SetInfoAboutRoomMooks()
    {
        if (enemyContainer != null)
        {
            roomMooks.AddRange(enemyContainer.GetComponentsInChildren<BaseMook>());
            roomMookCount = roomMooks.Count;
        }
        else
        {
            roomMookCount = 0;
        }
    }

    private void OnEnable()
    {
        if (roomMookCount > 0)
        {
            for (int i = 0; i < roomMooks.Count; i++)
            {
                roomMooks[i].onMookDeath += decrementMooksFromRoom;
            }
            SetDoorsLockState(true);
        }
    }

    private void SetDoorsLockState(bool state)
    {
        for (int i = 0; i < doorLocations.Count; i++)
        {
            doorLocations[i].Location.GetComponent<TestPlopper>().SetLockState(state);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < roomMooks.Count; i++)
        {
            roomMooks[i].onMookDeath -= decrementMooksFromRoom;
        }
    }

    private void decrementMooksFromRoom()
    {
        roomMookCount--;
        if (roomMookCount <= 0)
        {
            SetDoorsLockState(false);
        }
    }

    private void Awake()
    {
        RoomPrefab = GetComponent<GameObject>();
    }
    public Room getNeighbor(NeighborType neighborType)
    {
        Cell neighborCell;
        _cell.NeighborCells.TryGetValue(neighborType, out neighborCell);
        if (neighborCell == null) return null;
        return ProceduralGeneration.Singleton.GetRoomByCell(_cell.NeighborCells[neighborType]);
    }
    public GameObject RoomPrefab { get => roomPrefab; set => roomPrefab = value; }
    public int CellSize { get => _cellSize; set => _cellSize = value; }
    public Cell Cell { get => _cell; set => _cell = value; }

    public void WarpPlayerToDoorLocation(NeighborType neighborType)
    {
        PlayerController.Singleton.transform.position = roomDoors[neighborType].transform.position+roomDoors[neighborType].transform.TransformDirection(Vector3.forward);
        //The change in position wont be updated correctly if changes in transforms are not flushed correctly
        Physics.SyncTransforms();
    }
}
[System.Serializable]
class RoomNeighbor
{
    [SerializeField] NeighborType neighborType;
    [SerializeField] Room neighborRoom;

    public Room NeighborRoom { get => neighborRoom; set => neighborRoom = value; }
    public NeighborType NeighborType { get => neighborType; set => neighborType = value; }
}
[System.Serializable]
class DoorLocation
{
    [SerializeField] NeighborType neighborType;
    [SerializeField] GameObject location;

    public NeighborType NeighborType { get => neighborType; set => neighborType = value; }
    public GameObject Location { get => location; set => location = value; }
}