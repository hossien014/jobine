using Abed.Utils;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding 
{
    gridG<PathNod> grid;
    List<PathNod> OpenList;
    List<PathNod> CloseList;
    public List<PathNod> allnod;
    int Move_Straigh_cost=10;
    int Move_diagonl_cost=14;
    public PathFinding(int width,int height)
    {
        grid = new gridG<PathNod>(width, height, 20, new Vector3(-8, -4), (gridG<PathNod> g, int x, int y) => { return new PathNod(g,x,y); });
    }
    public List<Vector3> FindPath(Vector3 start,Vector3 end)
    {
        grid.GetXY(start, out int xStart, out int yStart);
        grid.GetXY(end, out int xend, out int yend);
        List<PathNod> Paths= FindPath(xStart, yStart, xend, yend);
        List<Vector3> pathVectors = new List<Vector3>();
        foreach(PathNod path in Paths)
        { 
            Vector3 p= grid.GetWorldPosition(path.x, path.y);
            Vector3 size = new Vector3(grid.GetCellSize() / 2, grid.GetCellSize() / 2);
            p += size;
            pathVectors.Add(p); 
        }
        if (pathVectors == null) return null;
        return pathVectors;
    }

    public List<PathNod> FindPath(int startX,int StartY,int EndX,int EndY)
    {
        PathNod startNode = grid.GetGridObject(startX, StartY);
        PathNod EndNod = grid.GetGridObject(EndX, EndY);
        OpenList = new List<PathNod> { startNode };
        CloseList = new List<PathNod>();
        InitiatePathNodes();
        InitiateStartNode(startNode, EndNod);
        while (OpenList.Count > 0)
        {
            PathNod currentNode = GetLowestFCostNode(OpenList);
            if (currentNode == EndNod)
            {
                return CaluclatePath(EndNod);
            }
            OpenList.Remove(currentNode);
            CloseList.Add(currentNode);
            AddNeighbersToList(EndNod, currentNode);
        }
        return null;
    }

    private void AddNeighbersToList(PathNod EndNod, PathNod currentNode)
    {
        foreach (PathNod neighber in GetNeighberList(currentNode))
        {
            if (neighber.blocked)
            {
                if (!CloseList.Contains(neighber)) { CloseList.Add(neighber); }
                continue;
            }
            if (CloseList.Contains(neighber)) continue;

            int TentativeGcost = currentNode.Gcost + CalculateDistanceCost(currentNode, neighber);
            if (TentativeGcost < neighber.Gcost)
            {
                neighber.ExporldForm = currentNode;
                neighber.Gcost = TentativeGcost;
                neighber.Hcost = CalculateDistanceCost(neighber, EndNod);
                neighber.calculateFcost();
                if (!OpenList.Contains(neighber))
                {
                    OpenList.Add(neighber);
                }
            }
        }
    }
    private void InitiateStartNode(PathNod startNode, PathNod EndNod)
    {
        startNode.Gcost = 0;
        startNode.Hcost = CalculateDistanceCost(startNode, EndNod);
        startNode.calculateFcost();
    }
    private void InitiatePathNodes()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNod pathnod = grid.GetGridObject(x, y);
                pathnod.Gcost = int.MaxValue;
                pathnod.calculateFcost();
                pathnod.ExporldForm = null;
            }
        }
    }

    private int CalculateDistanceCost(PathNod a, PathNod b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance); // Straghit move
        return Move_diagonl_cost * Mathf.Min(xDistance, yDistance) + Move_Straigh_cost * remaining;
    }
    List<PathNod> CaluclatePath(PathNod endNod)
    {
        List<PathNod> path = new List<PathNod>();
        path.Add(endNod);
        PathNod CurrentNod = endNod;
       
        while (CurrentNod.ExporldForm != null)
        {
            path.Add(CurrentNod.ExporldForm);
            CurrentNod = CurrentNod.ExporldForm;
        }
        path.Reverse();
        return path;
    }
       List<PathNod> GetNeighberList(PathNod currentNode)
    {
        List<PathNod> neighbourList = new List<PathNod>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }
    PathNod GetNode(int x,int y)
    {
        return grid.GetGridObject(x, y); 
    }

    private PathNod GetLowestFCostNode(List<PathNod> pathNodeList)
    {
        PathNod lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].Fcost < lowestFCostNode.Fcost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
   public gridG<PathNod> GetGrid()
    {
        return grid;
    }

}
