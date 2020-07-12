public class AStarMapNode_Obstacle : AStarMapNode
{
    public AStarMapNode_Obstacle(int x, int y) : base(x,y)
    {}
    public override bool IsObstacle()
    {
        return true;
    }
}