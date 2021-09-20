using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Stores references to all of the possible rooms, in a list currently
/// sorted by room size.
/// Should probably later on be also be sorted by room type (shop, fight room, boss room, etc). 
/// But this should be fine for an alpha
/// </summary>
public class RoomGen : MonoBehaviour
{

    [SerializeField] List<GameObject> allRooms = new List<GameObject>();
    [SerializeField] Dictionary<RoomType, List<GameObject>> roomsByRoomType = new Dictionary<RoomType, List<GameObject>>();
    private void Awake()
    {
        for (int i = 0; i < allRooms.Count; i++)
        {
            List<GameObject> listForRoomType;
            roomsByRoomType.TryGetValue(allRooms[i].GetComponent<Room>().RoomType, out listForRoomType);
            if (listForRoomType == null)
            {
                listForRoomType = new List<GameObject>();
                roomsByRoomType[allRooms[i].GetComponent<Room>().RoomType] = listForRoomType;
            }
            listForRoomType.Add(allRooms[i]);
        }
    }
    public GameObject createRoomForCell(Cell cell)
    {
        int random = Random.Range(0, roomsByRoomType[cell.RoomType].Count);
        GameObject go = Instantiate(roomsByRoomType[cell.RoomType][random]);
        go.GetComponent<Room>().Cell = cell;
        ProceduralGeneration.Singleton.AddCellToRoomInformation(cell, go.GetComponent<Room>());
        return go;
    }
}
