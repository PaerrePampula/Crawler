//IMinimumHeapables can be used the minimum heap implentation.
//The interface makes it so that the node with the smallest cellweight is 
//Considered to be the top member of the minimum heap.
/// <summary>
/// All checked nodes in the pathfinding algorithm are recorded in node records. 
/// They are used to save the cost to a specific cell so far, and to record their connections and visit state in the algorithm.
/// </summary>
class NodeRecord : IMinimumHeapable
{
    Cell cell;
    Connection connection;
    VisitState visitState;
    int costSoFar;
    public Connection Connection { get => connection; set => connection = value; }    
    public int CostSoFar { get => costSoFar; set => costSoFar = value; }    public Cell Cell { get => cell; set => cell = value; }
    public VisitState VisitState { get => visitState; set => visitState = value; }
    public int getElementValue()
    {
        return Cell.CellWeight;
    }
}
//Visit state lessens the need for collections during the pathfinding algorithm
//Open state nodes are found on the min heap.  A node is considered closed if all of its connections are checked in the algorithm.
enum VisitState
{
    Open,
    Closed,

}