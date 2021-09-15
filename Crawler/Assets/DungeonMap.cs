using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMap : MonoBehaviour
{
    [SerializeField] GameObject mapNode;
    [SerializeField] GameObject endNode;
    [SerializeField] GameObject startNode;
    [SerializeField] Transform mapParent;
    Vector3 mousePos = Vector3.zero;
    Vector3 oldMousePos = Vector3.zero;
    float mapZoomAmount = 1;
    float mapCellSize = 50;
    private void OnEnable()
    {
        ProceduralGeneration.onMapGenerated += createMap;
    }
    private void OnDisable()
    {
        ProceduralGeneration.onMapGenerated -= createMap;
    }
    private void createMap(List<Cell> cellsInDungeon)
    {
        Vector2 offset = new Vector2(cellsInDungeon[0].X * mapCellSize + mapParent.transform.localPosition.x, cellsInDungeon[0].Y * mapCellSize + mapParent.transform.localPosition.y);
        for (int i = 0; i < cellsInDungeon.Count; i++)
        {

            GameObject toInstantiate = mapNode;
            if (cellsInDungeon[i].CellType == CellType.Start)
            {
                toInstantiate = startNode;
            }
            if (cellsInDungeon[i].CellType == CellType.End)
            {
                toInstantiate = endNode;
            }
            GameObject go = Instantiate(toInstantiate, mapParent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(mapCellSize/2f, mapCellSize/2f);
            go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector2(mapCellSize * cellsInDungeon[i].X, mapCellSize * cellsInDungeon[i].Y) - offset;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PanMap();
        ZoomMap();
    }

    private void ZoomMap()
    {
        float zoomAmount = Input.GetAxis("Mouse ScrollWheel");
        mapZoomAmount += zoomAmount;
        mapZoomAmount = Mathf.Clamp(mapZoomAmount, 0.4f, 3);
        if (zoomAmount != 0)
        {
            mapParent.GetComponent<RectTransform>().localScale = new Vector3(mapZoomAmount, mapZoomAmount, 0);
        }
    }

    private void PanMap()
    {
        if (Input.GetMouseButton(1))
        {
            mousePos = Input.mousePosition;
            mapParent.transform.position = mapParent.transform.position - (oldMousePos - mousePos);
        }
    }

    private void FixedUpdate()
    {
        oldMousePos = Input.mousePosition;
    }
}
