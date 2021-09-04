using UnityEngine;

public class Connection
{
    int _connectionCost;
    Vector2 _fromNode;
    Vector2 _toNode;
    //Cost is generated during the construction of connection class instances. Depends on a cells' weight made during the initial graphing
    public Connection(int connectionCost, Vector2 fromNode, Vector2 toNode)
    {
        ConnectionCost = connectionCost;
        FromNode = fromNode;
        if (toNode != null)
            ToNode = toNode;
    }
    public int ConnectionCost { get => _connectionCost; set => _connectionCost = value; }
    public Vector2 FromNode { get => _fromNode; set => _fromNode = value; }
    public Vector2 ToNode { get => _toNode; set => _toNode = value; }


}