using UnityEngine;

public abstract class AStarMapNode
{
    private int x;
    private int y;
    public AStarMapNode(int inX, int inY)
    {
        x = inX;
        y = inY;
    }
    public abstract bool IsObstacle();

    public Vector2Int GetPosition()
    {
        return new Vector2Int(x,y);
    }
}