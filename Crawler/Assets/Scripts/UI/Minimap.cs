using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public delegate void MapGenerationComplete();
    public static event MapGenerationComplete onMapGenerationComplete;
    static Minimap singleton;
    public static Minimap Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<Minimap>();
            }
            return singleton;
        }
    }

    public Dictionary<Vector2, MapCell> DungeonCells { get => dungeonCells; set => dungeonCells = value; }
    [SerializeField] GameObject mapNode;
    [SerializeField] GameObject endNode;
    [SerializeField] GameObject startNode;
    [SerializeField] GameObject storeNode;
    [SerializeField] GameObject playerIcon;
    GameObject instantiatedPlayerIcon;
    [SerializeField] Transform mapParent;
    float mapZoomAmount = 1;
    float mapCellSize = 50;
    Vector2 offset;
    Dictionary<Vector2, MapCell> dungeonCells = new Dictionary<Vector2, MapCell>();

    private void OnEnable()
    {
        ProceduralGeneration.onGenerationComplete += createMap;
        CurrentRoomManager.onPlayerRoomSet += setPlayerIconToCurrentLocation;
        instantiatedPlayerIcon = Instantiate(playerIcon);
    }
    private void setPlayerIconToCurrentLocation(Room setRoom)
    {
        instantiatedPlayerIcon.transform.SetParent(dungeonCells[new Vector2(setRoom.Cell.X, setRoom.Cell.Y)].transform, false);
        instantiatedPlayerIcon.SetActive(true);
        mapParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset.x - (setRoom.Cell.X * mapCellSize), offset.y - (setRoom.Cell.Y * mapCellSize));

    }

    private void OnDestroy()
    {
        CurrentRoomManager.onPlayerRoomSet -= setPlayerIconToCurrentLocation;
        ProceduralGeneration.onGenerationComplete -= createMap;
    }
    private void createMap(List<Cell> cellsInDungeon)
    {
        //The start node will be located in the middle of the transform for the map container, all other cells of the map are offset by this
        offset = new Vector2(cellsInDungeon[0].X * mapCellSize + mapParent.transform.localPosition.x, cellsInDungeon[0].Y * mapCellSize + mapParent.transform.localPosition.y);
        for (int i = 0; i < cellsInDungeon.Count; i++)
        {

            GameObject toInstantiate = mapNode;
            if (cellsInDungeon[i].RoomType == RoomType.Start)
            {
                toInstantiate = startNode;
            }
            if (cellsInDungeon[i].RoomType == RoomType.BossBattle)
            {
                toInstantiate = endNode;
            }
            if (cellsInDungeon[i].RoomType == RoomType.Shop)
            {
                toInstantiate = storeNode;
            }
            GameObject go = Instantiate(toInstantiate, mapParent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(mapCellSize / 2f, mapCellSize / 2f);
            go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector2(mapCellSize * cellsInDungeon[i].X, mapCellSize * cellsInDungeon[i].Y) - offset;
            MapCell mapCell = go.GetComponent<MapCell>();
            mapCell.Initialize(cellsInDungeon[i]);
            DungeonCells.Add(new Vector2(cellsInDungeon[i].X, cellsInDungeon[i].Y), mapCell);
        }

    }
}
