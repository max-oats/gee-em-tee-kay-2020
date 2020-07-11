public class WorldTree : BaseEntity
{
    public override void TriggerInteract(InteractParams interactParams)
    {
        if (interactParams.holdingAncestor)
        {
            // Plant ancestor
        }
        else if (interactParams.holdingWater)
        {
            // Water tree
        }
    }
}
