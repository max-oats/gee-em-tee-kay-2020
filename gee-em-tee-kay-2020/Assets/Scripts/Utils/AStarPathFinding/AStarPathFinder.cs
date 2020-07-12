using UnityEngine;
using System.Collections.Generic;

public class AStarPathFinder
{
    private AStarMapNode[,] map;
    private Vector2Int goalLocation;

    private List<AStarMapNode_Terrain> openNodes = new List<AStarMapNode_Terrain>();
    private List<AStarMapNode> closedNodes = new List<AStarMapNode>();

    public void Init(Vector2Int size, Vector2Int start, Vector2Int goal)
    {
        openNodes.Clear();
        closedNodes.Clear();

        map = new AStarMapNode[size.x, size.y];
        goalLocation = goal;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                map[i,j] = new AStarMapNode_Terrain(i, j, 10 * (int)Vector2Int.Distance(goalLocation, new Vector2Int(i,j)));
            }
        }

        AStarMapNode_Terrain startNode = map[start.x, start.y] as AStarMapNode_Terrain;
        startNode.SetIsStart();
        openNodes.Add(startNode);
    }

    public void PlaceObstacleAt(int x, int y)
    {
        map[x,y] = new AStarMapNode_Obstacle(x,y);
    }

    public List<Direction4> GetPath4()
    {
        while (openNodes.Count > 0)
        {
            AStarMapNode_Terrain current = SelectNextOpenNode();
            if (current.GetPosition() == goalLocation)
            {
                return GeneratePath4From(current);
            }

            openNodes.Remove(current);
            closedNodes.Add(current);

            foreach (AStarMapNode_Terrain neighbour in GetNeighbours4(current))
            {
                neighbour.TrySetParent(current, 10);
                if (!openNodes.Contains(neighbour))
                {
                    openNodes.Add(neighbour);
                }
            }
        }

        return null;
    }

    public List<Direction8> GetPath8()
    {
        // Implement
        return null;
    }

    AStarMapNode_Terrain SelectNextOpenNode()
    {
        int smallestFCost = int.MaxValue;
        List<AStarMapNode_Terrain> nodesWithSmallestFCost = new List<AStarMapNode_Terrain>();
        foreach (AStarMapNode_Terrain node in openNodes)
        {
            if (node.GetFCost() < smallestFCost)
            {
                smallestFCost = node.GetFCost();
                nodesWithSmallestFCost.Clear();
                nodesWithSmallestFCost.Add(node);
            }
            else if (node.GetFCost() == smallestFCost)
            {
                nodesWithSmallestFCost.Add(node);
            }
        }

        if (nodesWithSmallestFCost.Count == 1)
        {
            return nodesWithSmallestFCost[0];
        }

        int smallestHCost = int.MaxValue;
        AStarMapNode_Terrain nodeWithSmallestHCost = null;
        foreach (AStarMapNode_Terrain node in nodesWithSmallestFCost)
        {
            if (node.GetHCost() < smallestHCost)
            {
                smallestHCost = node.GetHCost();
                nodeWithSmallestHCost = node;
            }
        }

        return nodeWithSmallestHCost;
    }

    List<AStarMapNode_Terrain> GetNeighbours4(AStarMapNode_Terrain node)
    {
        List<AStarMapNode_Terrain> neighbours = new List<AStarMapNode_Terrain>();
        Vector2Int position = node.GetPosition();

        Vector2Int positionToCheck = new Vector2Int(position.x-1, position.y);
        if (IsOpenTerrainNeighbour(positionToCheck))
        {
            neighbours.Add(map[positionToCheck.x, positionToCheck.y] as AStarMapNode_Terrain);
        }

        positionToCheck = new Vector2Int(position.x+1, position.y);
        if (IsOpenTerrainNeighbour(positionToCheck))
        {
            neighbours.Add(map[positionToCheck.x, positionToCheck.y] as AStarMapNode_Terrain);
        }

        positionToCheck = new Vector2Int(position.x, position.y-1);
        if (IsOpenTerrainNeighbour(positionToCheck))
        {
            neighbours.Add(map[positionToCheck.x, positionToCheck.y] as AStarMapNode_Terrain);
        }

        positionToCheck = new Vector2Int(position.x, position.y+1);
        if (IsOpenTerrainNeighbour(positionToCheck))
        {
            neighbours.Add(map[positionToCheck.x, positionToCheck.y] as AStarMapNode_Terrain);
        }

        return neighbours;
    }

    bool IsOpenTerrainNeighbour(Vector2Int positionToCheck)
    {
        return IsValidPosition(positionToCheck) && !map[positionToCheck.x, positionToCheck.y].IsObstacle() && !closedNodes.Contains(map[positionToCheck.x, positionToCheck.y]);
    }

    List<AStarMapNode_Terrain> GetNeighbours8(AStarMapNode_Terrain node)
    {
        // Implement
        return null;
    }

    List<Direction4> GeneratePath4From (AStarMapNode_Terrain goal)
    {
        List<Direction4> path = new List<Direction4>();

        AStarMapNode_Terrain parent = goal.GetParent();
        AStarMapNode_Terrain current = goal;
        while (parent != null)
        {
            if (FromDirection8(DirectionFromFirstToSecond(parent, current)) is Direction4 dir)
            {
                path.Add(dir);
            }
            else 
            {
                Debug.LogError("Found Direction8 when looking for Direction4s");
                return null;
            }
        }

        path.Reverse();
        return path;
    }

    List<Direction8> GeneratePath8From (AStarMapNode_Terrain goal)
    {
        // Implement
        return null;
    }

    bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < map.GetLength(0) && position.y < map.GetLength(1);
    }

    Direction8 DirectionFromFirstToSecond(AStarMapNode_Terrain from, AStarMapNode_Terrain to)
    {
       Vector2Int direction = to.GetPosition() - from.GetPosition();

       if (direction == Vector2Int.up)
       {
           return Direction8.North;
       }
       if (direction == Vector2Int.right)
       {
           return Direction8.East;
       }
       if (direction == Vector2Int.down)
       {
           return Direction8.South;
       }
       if (direction == Vector2Int.left)
       {
           return Direction8.West;
       }

       return Direction8.NorthEast;
    }

    Direction4? FromDirection8(Direction8 dir)
    {
        if (dir == Direction8.North)
        {
            return Direction4.North;
        }
        if (dir == Direction8.East)
        {
            return Direction4.East;
        }
        if (dir == Direction8.South)
        {
            return Direction4.South;
        }
        if (dir == Direction8.West)
        {
            return Direction4.West;
        }

        return null;
    }
}