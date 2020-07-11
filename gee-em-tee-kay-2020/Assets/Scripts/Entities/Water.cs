public class Water : BaseEntity
{
    public override void TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.holdingAncestor)
        {
            // Gather water
        }
    }
}
