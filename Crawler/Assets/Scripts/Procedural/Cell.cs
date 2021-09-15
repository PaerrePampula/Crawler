using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    int _x;
    int _y;
    CellType cellType;
    int cellWeight;
    bool _currentlyUsedOnMap;

    //Listing of all possible neighbors
    //### A|AR ##
    //#LT X X RT #
    //##L X X R ##
    //### B|BR ##
    //###########
    //The enum contains the possible neighbor types, also detailed above
    //Diagonally to the above or below nodes are not considered neighbors,
    //Since there is no way to have those rooms have connecting doors without
    //Some extra trickery involved.
    Dictionary<NeighborType, Cell> neighborCells = new Dictionary<NeighborType, Cell>();
    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }
    //Once the node map is done, some branches are generated. Once some branches are generated, meaning the connections need to be updated.
    public void RegenerateConnections()
    {
        GetConnections();
    }
    public List<Connection> GetConnections()
    {
        List<Connection> connections = new List<Connection>();
        //So for x-1 and x-1+2, the cell on the left and the cell on the right
        for (int xPos = X-1; xPos <= X+1; xPos+=2)
        {
            NeighborType neighborType = (xPos < X) ? NeighborType.Left : NeighborType.Right;
            //Make sure that the check doesnt go over the boundaries of the generated nodes.
            if (xPos >= 0 && xPos <= ProceduralGeneration.Singleton.MapWidth)
            {
                Connection c = FindNeighborFor(xPos, Y);

                if (c != null)
                {
                    connections.Add(c);
                    NeighborCells[neighborType] = ProceduralGeneration.Singleton.CellsTable[c.ToNode];
                }
            }
        }
        //So for y-1 and y-1+2, the cell below, and the cell above
        for (int yPos = Y-1; yPos <= Y+1; yPos+=2)
        {
            NeighborType neighborType = (yPos < Y) ? NeighborType.Below : NeighborType.Above;
            //Make sure that the check doesnt go over the boundaries of the generated nodes.
            if (yPos >= 0 && yPos <= ProceduralGeneration.Singleton.MapHeight)
            {
                Connection c = FindNeighborFor(X, yPos);

                if (c != null)
                {
                    connections.Add(c);
                    NeighborCells[neighborType] = ProceduralGeneration.Singleton.CellsTable[c.ToNode];
                }
            }
        }
        return connections;
    }

    private Connection FindNeighborFor(int valueToCompareX, int valueToCompareY)
    {
        Connection connection = null;
        if (valueToCompareX == X && valueToCompareY == Y) return null;
        if (ProceduralGeneration.Singleton.CellsTable.ContainsKey(new Vector2(valueToCompareX, valueToCompareY)))
        {
            Cell connectionCell = ProceduralGeneration.Singleton.CellsTable[new Vector2(valueToCompareX, valueToCompareY)];
            connection = new Connection(connectionCell.cellWeight, new Vector2(X, Y), new Vector2(valueToCompareX, valueToCompareY));
        }
        return connection;
    }

    public int getElementValue()
    {
        return cellWeight;
    }

    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }
    public int CellWeight { get => cellWeight; set => cellWeight = value; }

    public bool CurrentlyUsedOnMap { get => _currentlyUsedOnMap; set => _currentlyUsedOnMap = value; }

    public Dictionary<NeighborType, Cell> NeighborCells { get => neighborCells; set => neighborCells = value; }
    internal CellType CellType { get => cellType; set => cellType = value; }
}
/// <summary>
/// Used by procedural generation cells and rooms. For each room, a location for a 
/// door should be determined using neighbor type.
/// Each room is not required to have all of the connecting doors to be valid.
/// </summary>
public enum NeighborType
{
    Left,
    Right,
    Above,
    Below,
}
enum CellType
{
    Normal,
    Start,
    End
}