public class AStarMapNode_Terrain : AStarMapNode
{
    // Distance of node from starting node
    private int gCost = int.MaxValue;

    // Distance of node from goal node
    private int hCost = 0;

    // gCost + hCost
    private int fCost = 0;

    private AStarMapNode_Terrain parent = null;

    public AStarMapNode_Terrain(int inH)
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
        return fCost;
    }

    void SetGCost(int inG)
    {
        gCost = inG;
        fCost = gCost + hCost;
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