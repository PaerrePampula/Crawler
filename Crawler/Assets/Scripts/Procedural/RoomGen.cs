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

    [SerializeField] List<GameObject> allNodeVisuals = new List<GameObject>();
    [SerializeField] List<GameObject> allRooms = new List<GameObject>();

    public GameObject createNodeVisualizationForCell(Cell cell)
    {
        int random = Random.Range(0, allNodeVisuals.Count);
        GameObject go = Instantiate(allNodeVisuals[random]);
        return go;
    }
    public GameObject createRoomForCell(Cell cell)
    {
        int random = Random.Range(0, allRooms.Count);
        GameObject go = Instantiate(allRooms[random]);
        go.GetComponent<Room>().Cell = cell;
        ProceduralGeneration.Singleton.AddCellToRoomInformation(cell, go.GetComponent<Room>());
        return go;
    }
}
