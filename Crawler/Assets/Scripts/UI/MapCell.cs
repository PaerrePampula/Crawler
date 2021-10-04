using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCell : MonoBehaviour
{
    bool discoveredByPlayer;
    bool visitedByPlayer = false;
    //Actual cell on the procedural map
    Cell _cell;
    Image cellImage;
    Color32 originalColor;


    public Cell Cell { get => _cell; set => _cell = value; }
    private void Awake()
    {

        if (discoveredByPlayer == false)
        {
            gameObject.SetActive(false);
        }
        cellImage = GetComponent<Image>();

    }
    public void Initialize(Cell cell)
    {

        cellImage = GetComponent<Image>();
        originalColor = cellImage.color;
        Cell = cell;
        CurrentRoomManager.onPlayerRoomSet += checkIfDiscoveredAndIfIsCurrentRoom;
        Cell.onCellDiscover += discoverMapCell;
    }


    private void checkIfDiscoveredAndIfIsCurrentRoom(Room setRoom)
    {

        //Player room is the same room as the cell represented by this ui element
        if (setRoom.Cell == Cell)
        {
            discoverMapCell();
            cellImage.color = Color.red;
            visitedByPlayer = true;
            foreach (Cell cell in Cell.NeighborCells.Values)
            {
                //Avoid having to find the actual mapcells from some collection by just invoking 
                //discovery in the other cells
                cell.InvokeCellDiscover();
            }
        }
        else
        {
            if (cellImage != null)
            {
                if (!visitedByPlayer)
                {
                    cellImage.color = new Color32((byte)(originalColor.r - 125), (byte)(originalColor.g - 125), (byte)(originalColor.b - 125), 255);
                }
                else
                {
                    cellImage.color = originalColor;
                }
            }


        }
    }
    private void OnDisable()
    {
        
    }
    private void OnDestroy()
    {
        if (Cell != null)
        {
            Cell.onCellDiscover -= discoverMapCell;
        }
        CurrentRoomManager.onPlayerRoomSet -= checkIfDiscoveredAndIfIsCurrentRoom;
    }
    void discoverMapCell()
    {
        discoveredByPlayer = true;

        gameObject.SetActive(true);
    }
}
