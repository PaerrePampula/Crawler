
using UnityEngine;
//Used by all prefabs marked as rooms, rooms should have (currently) a size
//of either 1 or 2. 1 size room will be depicted as a 1x1 room on the map
//2 room will be depicted as 2x2 size on the map
public class Room : MonoBehaviour
{
    [SerializeField] int _cellSize;

    GameObject roomPrefab;
    private void Awake()
    {
        RoomPrefab = GetComponent<GameObject>();
    }

    public GameObject RoomPrefab { get => roomPrefab; set => roomPrefab = value; }
    public int CellSize { get => _cellSize; set => _cellSize = value; }
}
