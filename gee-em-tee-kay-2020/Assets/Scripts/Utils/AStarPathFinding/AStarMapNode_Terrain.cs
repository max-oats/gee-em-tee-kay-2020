public class AStarMapNode_Terrain : AStarMapNode
{
    // Distance of node from starting node
    private int gCost = int.MaxValue;

    // Distance of node from goal node
    private int hCost = 0;

    private AStarMapNode_Terrain parent = null;

    public AStarMapNode_Terrain(int inX, int inY, int inH) : base(inX, inY)
    {
        hCost = inH;
    }

    public override bool IsObstacle()
    {
        return false;
    }

    public int GetHCost()
    {
        return hCost;
    }

    public int GetFCost()
    {
        return gCost + hCost;
    }

    void SetGCost(int inG)
    {
        gCost = inG;
    }

    public void SetIsStart()
    {
        gCost = 0;
    }

    public AStarMapNode_Terrain GetParent()
    {
        return parent;
    }

    public void TrySetParent(AStarMapNode_Terrain inParent, int stepCostFromParent)
    {
        if (inParent.gCost + stepCostFromParent < gCost)
        {
            parent = inParent;
            SetGCost(inParent.gCost + stepCostFromParent);
        }
    }
}