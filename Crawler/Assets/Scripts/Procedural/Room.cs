
using System.Collections.Generic;
using UnityEngine;
//Used by all prefabs marked as rooms, rooms should have (currently) a size
//of either 1 or 2. 1 size room will be depicted as a 1x1 room on the map
//2 room will be depicted as 2x2 size on the map
public class Room : MonoBehaviour
{
    Dictionary<NeighborType, Room> roomNeighbors = new Dictionary<NeighborType, Room>();
    Dictionary<NeighborType, GameObject> neighborToDoorLocation = new Dictionary<NeighborType, GameObject>();
    [SerializeField] List<DoorLocation> doorLocations = new List<DoorLocation>();
    [SerializeField] List<RoomNeighbor> roomNeighborsList = new List<RoomNeighbor>();
    [SerializeField] int _cellSize;
    GameObject roomPrefab;
    private void Start()
    {
        if (this != CurrentRoomManager.Singleton.currentRoom) gameObject.SetActive(false);
        //Save the list as a dictionary to make lookups a bit better
        for (int i = 0; i < roomNeighborsList.Count; i++)
        {
            roomNeighbors.Add(roomNeighborsList[i].NeighborType, roomNeighborsList[i].NeighborRoom);
        }
        for (int i = 0; i < doorLocations.Count; i++)
        {
            neighborToDoorLocation.Add(doorLocations[i].NeighborType, doorLocations[i].Location);
        }

    }
    private void Awake()
    {
        RoomPrefab = GetComponent<GameObject>();
    }
    public GameObject RoomPrefab { get => roomPrefab; set => roomPrefab = value; }
    public int CellSize { get => _cellSize; set => _cellSize = value; }
    public void WarpPlayerToDoorLocation(NeighborType neighborType)
    {
        PlayerController.Singleton.transform.position = neighborToDoorLocation[neighborType].transform.position;
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