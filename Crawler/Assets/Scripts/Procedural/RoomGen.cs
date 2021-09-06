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
    Dictionary<int, List<GameObject>> allRoomsDict = new Dictionary<int, List<GameObject>>();
    [SerializeField] List<GameObject> AllRooms = new List<GameObject>();
    private void Awake()
    {
        //Lets add all the rooms in the inspector to the dictionary
        for (int i = 0; i < AllRooms.Count; i++)
        {
            Room room = AllRooms[i].GetComponent<Room>();
            //First we check if an instance of list with certain width and height rooms exist
            if (!allRoomsDict.ContainsKey((room.CellSize)))
            {
                //If it doesnt exist, create one and add
                List<GameObject> newTypeOfRoomListing = new List<GameObject>();
                allRoomsDict.Add((room.CellSize), newTypeOfRoomListing);
                newTypeOfRoomListing.Add(AllRooms[i]);
            }
            else
            {
                //A list was found, add to this list the current iteration.
                allRoomsDict[(room.CellSize)].Add(AllRooms[i]);
            }

        }
    }
    public GameObject createRoomForCell(Cell cell)
    {
        int random = Random.Range(0, allRoomsDict[cell.CellSize].Count);
        GameObject go = Instantiate(allRoomsDict[cell.CellSize][random]);
        return go;
    }
}
