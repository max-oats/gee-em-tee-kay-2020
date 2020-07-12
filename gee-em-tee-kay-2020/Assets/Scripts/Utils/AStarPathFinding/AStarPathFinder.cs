using UnityEngine;
using System.Collections.Generic;

public class AStarPathFinder
{
    private AStarMapNode[,] map;
    private Vector2Int goalLocation;

    private List<AStarMapNode_Terrain> openNodes = new List<AStarMapNode_Terrain>();
    private List<AStarMapNode_Terrain> closedNodes = new List<AStarMapNode_Terrain>();

    public void Init(Vector2Int size, Vector2Int start, Vector2Int goal)
    {
        map = new AStarMapNode[size.x, size.y];
        goalLocation = goal;

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                map[i,j] = new AStarMapNode_Terrain(10 * (int)Vector2Int.Distance(goalLocation, new Vector2Int(i,j)));
            }
        }

        openNodes.Add(map[start.x, start.y] as AStarMapNode_Terrain);
    }

    public void PlaceObstacleAt(int x, int y)
    {
        map[x,y] = new AStarMapNode_Obstacle();
    }


    public Direction4[] GetPath4()
    {
        // Implement
        return null;
    }

    public Direction8[] GetPath8()
    {
        // Implement
        return null;
    }
}