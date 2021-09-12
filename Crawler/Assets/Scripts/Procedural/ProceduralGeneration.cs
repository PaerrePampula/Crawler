﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class ProceduralGeneration : MonoBehaviour
{
    static ProceduralGeneration singleton;
    public static ProceduralGeneration Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<ProceduralGeneration>();
            }
            return singleton;
        }
    }
    public int MapWidth { get => maximumMapWidth; set => maximumMapWidth = value; }
    public int MapHeight { get => maximumMapHeight; set => maximumMapHeight = value; }
    public Dictionary<Vector2, Cell> ReadyCells { get => readyCells; set => readyCells = value; }
    public Dictionary<Vector2, Cell> AvailableSpaces { get => availableSpaces; set => availableSpaces = value; }
    //Graph generation collections
    //Spaces that are so far free to use to make a room
    Dictionary<Vector2, Cell> availableSpaces;
    //The above list but this one is used to loop through the elements to 
    //Empty out the above list by reserving spaces for different size rooms
    List<Vector2> availableSpacesAsAList;
    //These are rooms that are reserved for possible rooms on the map
    Dictionary<Vector2, Cell> readyCells;
    //Pathfinding collections
    //A minimum heap version of currently considered pathfinding cells
    MinHeap<NodeRecord> openPathFindingCells;
    //All records of rooms in the pathfinding algorithm so far.
    Dictionary<Vector2, NodeRecord> allNodeRecords;
    //The current cell in consideration of the algorithm for pathfinding
    NodeRecord currentRecord;
    //A pathfinded route from the beginning room to "boss room". Generated through the dijikstra algorithm.
    public List<Cell> pathFromStartToGoal = new List<Cell>();
    List<Cell> allCellsUsedByDungeon = new List<Cell>();
    Dictionary<Cell, Room> _cellToDoor = new Dictionary<Cell, Room>();
    [Header("Procedurally generated map parameters")]
    [SerializeField] int maximumMapWidth = 100;
    [SerializeField] int maximumMapHeight = 100;
    [SerializeField] float chanceForPathToBranch = 60f;
    [SerializeField] float chanceForBigRoomGeneration = 35f;
    [Tooltip("Dictates how long a path that isnt on the start->goal axis can maximally be.")]
    [SerializeField] int maxBranchingPathLength = 10;
    [Tooltip("Distance is measured in unity units.")]
    [SerializeField] float requiredDistanceBetweenStartRoomAndGoalRoom = 5;
    Vector2 goalSpace;
    Vector2 startSpace;

    private void Start()
    {

        ProcedurallyGenerateAMap();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }
    }
    private void ProcedurallyGenerateAMap()
    {
        //ALGORITHM DESCRIPTION
        /* Generates all the rooms of the game with some algorithms by:
         * 1. Save a location for the start, and the end. There is a set minimum distance between these two.
         * 2. Create a dictionary for all possible room locations
         * 3. Shuffle the afermentioned list. Loop the list until the end.
         * 4. On each loop, reserve the room location if it still can be reserved. Determine a size for the room, and reserve the extra space as well. Skip if room is already reserved
         * 5. Still on the loop, give each room a weight. This weight will be used in the pathfinding algorithm for the shortest path between start and goal. (gives some randomness to the shape of map).
         * 6. Fire up a version of dijikstras algorithm. Calculate path shortest to goal from start.
         * 7. Once this is done, loop through the path, making the rooms. 
         * 8. Through random chance, make some branching rooms.
         * The level is now done!
         * TODO:
         *      Make the actual level pieces generate on the map
         *      Add doors between rooms
         *      Add doors to branches, with some randomness whether they should have a connection to every adjancent branch
         *      Add connectivity between the room class to search specific neighbors of a cell for certain door connections
         * More info can be found from this stackexchange post, which inspired some general ideas of this particular algorithm.
         * https://gamedev.stackexchange.com/questions/148418/procedurally-generating-dungeons-using-predefined-rooms
         */

        //Initialize needed lists.
        openPathFindingCells = new MinHeap<NodeRecord>(maximumMapWidth * maximumMapHeight);
        AvailableSpaces = new Dictionary<Vector2, Cell>();
        availableSpacesAsAList = new List<Vector2>();
        ReadyCells = new Dictionary<Vector2, Cell>();
        allNodeRecords = new Dictionary<Vector2, NodeRecord>();

        //Create all cells for the map. starts off with all the cells being 1x1 in size.
        CreateInitialGraph();
        //Lets shuffle the list of available spaces
        FisherYatesShuffle(ref availableSpacesAsAList);
        //Create the first two nodes to be goal and start.
        preselectStartAndGoalLocationsFromGraph();
        //All of the other cells are now checked and generated in correct sizes. 
        //Cells that do not fit a changed graph wont be generated, and will be skipped.
        //Degenerativeness occurs if a cell tries to fit itself on a cell used by a 2x2 cell area.
        ReserveSpacesForAllGeneratedCellsAndResizeThem();
        //Generate a path using dijikstras algorithm to reach goal from the start cell
        GeneratePathToGoal();
        //once a path is done, the rooms can be generated on in the gameworld using the path generated by pathfinding
        MakeRooms();
    }

    private void GeneratePathToGoal()
    {
        //Make sure that the minheap is empty to prevent some possible bugs. Should always be empty before the algorithm runs
        openPathFindingCells.ClearHeap();
        //We start at the starting point, so lets make a record of that point as a noderecord
        NodeRecord startingRecord = new NodeRecord();
        startingRecord.Cell = readyCells[startSpace];
        startingRecord.CostSoFar = 0;
        //The node should have no cost. because you start at this node.
        allNodeRecords.Add(startSpace, startingRecord);
        //Add it to the minheap, currently, it should be the only element.
        openPathFindingCells.InsertKey(startingRecord);
        //Iterate through the minimum heap until its empty, or goal is reached.
        //Fail the algorithm if goal is not the currently inspected element after exiting the while loop
         while (openPathFindingCells.CurrentHeapSize > 0)
        {
            //The smallest element of the list should be the current record inspected
            currentRecord = openPathFindingCells.extractMinKey();
            //This code will exit if the current cell is the goal prematurely before the algorithm runs all the way the list of nodes to
            //save computing time. A suboptimal shortest route is good enough for most games, including this, especially when its only used to 
            //generate maps on runtime once per game.
            if (areSameCells(currentRecord.Cell, readyCells[goalSpace])) break;
            //Make a record of all the connecting nodes, then loop these.
            List<Connection> connections = currentRecord.Cell.GetConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                //Save the current cost + the cost to this connection
                int endNodeCost = currentRecord.CostSoFar + connections[i].ConnectionCost;
                //Cache the current connection as a record. If the record is null, it should be created. A null record means that this node 
                //Hasnt been checked yet even once.
                NodeRecord endNodeRecord;
                allNodeRecords.TryGetValue(connections[i].ToNode, out endNodeRecord);

                if (endNodeRecord != null)
                {
                    //The node can be on the closed node list, but a shorter path can still be found through it, even though it is a bit rare.
                    if (endNodeRecord.VisitState == VisitState.Closed)
                    {
                        if (endNodeRecord.CostSoFar > endNodeCost)
                        {
                            //We found a shorter route! update the node to be open yet again 
                            //and save the cost
                            endNodeRecord.VisitState = VisitState.Open;
                            openPathFindingCells.InsertKey(endNodeRecord);

                        }
                        else
                        {
                            //A longer route is found, continue next iteration.
                            continue;
                        }

                    }
                    else
                    {
                        //A longer or equal route was found that what is currently recorded. Skip this iteration.
                        if (endNodeRecord.CostSoFar <= endNodeCost) continue;

                    }
                }
                else
                {
                    //We reach here if the node is not found, that means we need to generate a record for this node!
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.Cell = readyCells[connections[i].ToNode];
                    allNodeRecords.Add(new Vector2(endNodeRecord.Cell.X, endNodeRecord.Cell.Y), endNodeRecord);
                    openPathFindingCells.InsertKey(endNodeRecord);
                }
                //we reach here if a more potentially optimal path was found for a connection, or if a new record was created.
                //Save the new cost for the record, plus the potentially better connection.
                endNodeRecord.CostSoFar = endNodeCost;
                endNodeRecord.Connection = connections[i];
                //The minheap might have gotten some violations, fix this
                openPathFindingCells.MinHeapify(0);

            }
            //After the node was checked for all connections, the node will be considered "closed."
            //the node is automatically removed from the list by popping earlier on the algorithm, so that is not necessary here.
            currentRecord.VisitState = VisitState.Closed;

        }
        if (areSameCells(currentRecord.Cell, readyCells[goalSpace]) == false)
        {
            //NO PATH!!! This is a possible error in the algorithm
            Debug.LogError("ROOM PROCEDURAL CREATION FAILURE! Current node in algorithm doesnt match the goal space of the algorithm");


        }
        else
        {
            //Time to loop through the path starting from the goal, and ending at the start.
            while (!areSameCells(currentRecord.Cell, readyCells[startSpace]))
            {
                //For each loop, add the connecting node to the path, then change the current node to be the other node in the connection
                pathFromStartToGoal.Add(readyCells[currentRecord.Connection.ToNode]);
                currentRecord = allNodeRecords[currentRecord.Connection.FromNode];
            }
            //The start space is the final node on the list.
            pathFromStartToGoal.Add(readyCells[startSpace]);
        }
        //reverse this list to make it go from start->goal.
        pathFromStartToGoal.Reverse();
    }

    private void CreateInitialGraph()
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                Vector2 vector = new Vector2(x, y);
                int randomWeight = Random.Range(1, 101);
                Cell cell = new Cell(x, y);
                //start off at a random cell weight from 1 to 100
                cell.CellWeight = randomWeight;
                //start off with a small cell, change it later on if random chance dictates so.
                cell.CellSize = 1;
                AvailableSpaces.Add(vector, cell);
                availableSpacesAsAList.Add(vector);

            }
        }
    }

    private void ReserveSpacesForAllGeneratedCellsAndResizeThem()
    {
        for (int i = 0; i < availableSpacesAsAList.Count; i++)
        {
            //The currently checked cell should be cached first, then checked if it exists anymore on the list.
            Cell cell;
            AvailableSpaces.TryGetValue(availableSpacesAsAList[i], out cell);

            //Basicly a cell that has already been reserved was found
            if (cell == null) continue;
            //The cell does exist on the list. Continue by saving the coordinates of this cell.
            int xCoord = (int)availableSpacesAsAList[i].x;
            int yCoord = (int)availableSpacesAsAList[i].y;
            //Then by random chance set in the inspector, make the room big if it passes the checks
            float roomIsBigChance = Random.Range(0f, 101);
            if (roomIsBigChance < chanceForBigRoomGeneration)
            {
                //The room needs space on the right, above, and diagonally above to be set to be a "big" 2x2 space on the grid map.
                if (AvailableSpaces.ContainsKey(new Vector2(xCoord + 1, yCoord)))
                {
                    if (AvailableSpaces.ContainsKey(new Vector2(xCoord, yCoord + 1)))
                    {
                        if (AvailableSpaces.ContainsKey(new Vector2(xCoord + 1, yCoord + 1)))
                        {
                            //HUGE ROOM SIZE == 2X2 THE NORMAL ROOM. Atleast on the map. The sizes are only for the map really, and to specify specific room types to be big.
                            cell.CellSize = 2;
                        }

                    }
                }
            }
            //Once the roomsize is determined, all the spaces are reserved for this room only.
            ReserveSpaces(xCoord, yCoord, cell.CellSize);
            //This is also saved on the ready list of cells for later use.
            ReadyCells.Add(new Vector2(xCoord, yCoord), cell);
        }
    }

    private void preselectStartAndGoalLocationsFromGraph()
    {
        //The first space from the shuffled list shall be reserved for the start
        startSpace = availableSpacesAsAList[0];
        ReadyCells.Add(startSpace, availableSpaces[startSpace]);
        availableSpaces[startSpace].CellSize = 2;
        ReserveSpaces((int)startSpace.x, (int)startSpace.y, availableSpaces[startSpace].CellSize);

        //Prevent degenerate maps from being made with there being set minimum distance between goal and start
        //The next index of the list shall be the goal, but change the index if above isnt satisfied
        int indexForGoal = 1;
        while (Vector2.Distance(availableSpacesAsAList[indexForGoal], startSpace) < requiredDistanceBetweenStartRoomAndGoalRoom)
        {
            indexForGoal++;
        }
        goalSpace = availableSpacesAsAList[indexForGoal];
        ReadyCells.Add(goalSpace, availableSpaces[goalSpace]);
        availableSpaces[goalSpace].CellSize = 2;
        ReserveSpaces((int)goalSpace.x, (int)goalSpace.y, availableSpaces[goalSpace].CellSize);
    }

    private void ReserveSpaces(int xCoord, int yCoord, int scale)
    {
        for (int x = 0; x < scale; x++)
        {
            for (int y = 0; y < scale; y++)
            {
                ReserveSpace(new Vector2(xCoord+x, yCoord+y));
            }
        }
    }

    public void MakeRooms()
    {
        //Do one iteration over the path to set all path members to be used, to prevent branching
        //paths thinking that the next location on path is usable as a branch
        for (int i = 0; i < pathFromStartToGoal.Count; i++)
        {
            pathFromStartToGoal[i].CurrentlyUsedOnMap = true;
            allCellsUsedByDungeon.Add(pathFromStartToGoal[i]);
        }
        //Once the paths are set to be used, iterate them, creating branching paths whenever chance dictates so
        for (int i = 0; i < pathFromStartToGoal.Count; i++)
        {
            //Create a visualization of this node for now as a cube.
            GameObject go = GetComponent<RoomGen>().createNodeVisualizationForCell(pathFromStartToGoal[i]);
            //Only create branching paths if the current node is not the goal or the start.
            if (i > 0 && i < pathFromStartToGoal.Count - 1)
            {
                GenerateBranchingPaths(pathFromStartToGoal[i]);
            }
            //Set the visualizing cube location to be the node location.
            go.transform.position = new Vector3(pathFromStartToGoal[i].X, 0, pathFromStartToGoal[i].Y);
        }
        for (int i = 0; i < allCellsUsedByDungeon.Count; i++)
        {
            GetComponent<RoomGen>().createRoomForCell(allCellsUsedByDungeon[i]);
        }
        for (int i = 0; i < allCellsUsedByDungeon.Count; i++)
        {
            allCellsUsedByDungeon[i].RegenerateConnections();

        }
        CurrentRoomManager.Singleton.currentRoom = _cellToDoor[allCellsUsedByDungeon[0]];
        _cellToDoor[allCellsUsedByDungeon[0]].gameObject.SetActive(true);

    }

    private void GenerateBranchingPaths(Cell cell, int currentBranchingPathCount = 0)
    {
        //Check if the branching path length is too deep, dont make the branching path any longer if it is so.
        if (currentBranchingPathCount >= maxBranchingPathLength) return;
        //Use random chance if a branch room is created, including already on a branching path.
        float randomChanceForNonPathRooms = Random.Range(0, 101);
        //If check succeeded, create the branching room
        if (randomChanceForNonPathRooms < chanceForPathToBranch)
        {
            //Get the nearby cells for this particular cell.
            List<Cell> adjancentCells = new List<Cell>();
            adjancentCells.AddRange(cell.NeighborCells.Values);
            //Shuffle the list a bit to make the branching path location vary side to side.
            FisherYatesShuffle(ref adjancentCells);
            //Iterate these cells for rooms that arent on the start->goal path, and create a branching room on any of these
            foreach (Cell neighbor in cell.NeighborCells.Values)
            {
                if (neighbor != null)
                {
                    if (neighbor.CurrentlyUsedOnMap) continue;
                    //Create a visualization for this branch
                    GameObject goAdjancent = GetComponent<RoomGen>().createNodeVisualizationForCell(neighbor);
                    goAdjancent.transform.position = new Vector3(neighbor.X, 0, neighbor.Y);
                    //The 2x2 room has 4x small cubes as the "room". Loop through them all to give the branching path a bit different color.
                    foreach (var rendered in goAdjancent.transform.GetComponentsInChildren<MeshRenderer>())
                    {
                        rendered.material.color = new Color32(0, 0, 255, 0);
                    }

                    neighbor.CurrentlyUsedOnMap = true;
                    allCellsUsedByDungeon.Add(neighbor);
                    //Increase the depth of this current branch by one.
                    currentBranchingPathCount++;

                    GenerateBranchingPaths(neighbor, currentBranchingPathCount);
                    //One branching room per room should be enough, so break the loop if this one room is created.
                }
            }


        }
    }

    void ReserveSpace(Vector2 spaceToReserve)
    {
        AvailableSpaces.Remove(spaceToReserve);
    }
    private void FisherYatesShuffle<T>(ref List<T> list)
    {
        //Just a standard fisher yates shuffle
        //https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        for (int i = 0; i < list.Count; i++)
        {
            int fisherYatesShuffleRandomIndex = Random.Range(0, list.Count - 1);
            T temporary = list[i];
            list[i] = list[fisherYatesShuffleRandomIndex];
            list[fisherYatesShuffleRandomIndex] = temporary;
        }
    }
    bool areSameCells(Cell a, Cell b)
    {
        if (a.X == b.X)
        {
            if (a.Y == b.Y)
            {
                return true;
            }
        }
        return false;
    }
    public Room GetRoomByCell(Cell cell)
    {
        Room room;
        _cellToDoor.TryGetValue(cell, out room);
        return room;
    }
    public void AddCellToRoomInformation(Cell cell, Room room)
    {
        _cellToDoor.Add(cell, room);
    }
}
