using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    int _x;
    int _y;
    int _cellSize;
    int cellWeight;
    bool _currentlyUsedOnMap;

    Dictionary<NeighborType, Cell> neighborCells = new Dictionary<NeighborType, Cell>();
    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }
    public List<Connection> GetConnections()
    {
        List<Connection> connections = new List<Connection>();
        for (int x = 0; x < CellSize; x++)
        {
            for (int y = 0; y < CellSize; y++)
            {
                int xPosInGrid = x + X;
                int yPosInGrid = y + Y;
                if (xPosInGrid - 1 >= 0)
                {
                    NeighborType neighborType = (y == Y) ? NeighborType.Left : NeighborType.LeftTop;
                    Connection c = FindNeighborFor( xPosInGrid - 1, yPosInGrid);
                    //You might have this checking the position at 1,0, but not being able to reach
                    //the position at 0,0 because its only checking the adjancent cell.
                    //adjust the search one longer if this is the case.
                    if (c == null)
                    {
                        c = FindNeighborFor(xPosInGrid - 2, yPosInGrid);
                    }
                    if (c != null)
                    {
                        connections.Add(c);
                        NeighborCells[neighborType]= ProceduralGeneration.Singleton.ReadyCells[c.ToNode];
                    }
                }
                if (xPosInGrid + 1 <= ProceduralGeneration.Singleton.MapWidth)
                {
                    NeighborType neighborType = (y == Y) ? NeighborType.Right : NeighborType.RightTop;
                    Connection c = FindNeighborFor( xPosInGrid + 1, yPosInGrid);
                    if (c != null)
                    {
                        connections.Add(c);
                        NeighborCells[neighborType] = ProceduralGeneration.Singleton.ReadyCells[c.ToNode];
                    }
                }
                if (yPosInGrid - 1 >= 0)
                {
                    NeighborType neighborType = (x == X) ? NeighborType.Below : NeighborType.BelowRight;
                    Connection c = FindNeighborFor(xPosInGrid, yPosInGrid - 1);
                    //You might have a position at 0,0 and this is trying to check e.g 1,0, but cant reach 0,0 because its 2x2 in size for example
                    //Do another check in this case
                    if (c == null)
                    {
                        c = FindNeighborFor(xPosInGrid, yPosInGrid - 2);
                    }
                    if (c != null)
                    {
                        connections.Add(c);
                        NeighborCells[neighborType] = ProceduralGeneration.Singleton.ReadyCells[c.ToNode];
                    }

                }
                if (yPosInGrid + 1 <= ProceduralGeneration.Singleton.MapHeight)
                {
                    NeighborType neighborType = (x == X) ? NeighborType.Above : NeighborType.AboveRight;
                    Connection c = FindNeighborFor(xPosInGrid, yPosInGrid + 1);
                    if (c != null)
                    {
                        connections.Add(c);
                        NeighborCells[neighborType] = ProceduralGeneration.Singleton.ReadyCells[c.ToNode];
                    }
                }

            }
        }
        return connections;
    }

    private Connection FindNeighborFor(int valueToCompareX, int valueToCompareY)
    {
        Connection connection = null;
        if (valueToCompareX == X && valueToCompareY == Y) return null;
        if (ProceduralGeneration.Singleton.ReadyCells.ContainsKey(new Vector2(valueToCompareX, valueToCompareY)))
        {
            Cell connectionCell = ProceduralGeneration.Singleton.ReadyCells[new Vector2(valueToCompareX, valueToCompareY)];
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
    public int CellSize { get => _cellSize; set => _cellSize = value; }
    public Dictionary<NeighborType, Cell> NeighborCells { get => neighborCells; set => neighborCells = value; }
}
public enum NeighborType
{
    Left,
    Right,
    Above,
    Below,
    LeftTop,
    RightTop,
    BelowRight,
    AboveRight,
}
