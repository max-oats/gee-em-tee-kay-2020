public class Ancestor : BaseEntity
{
    public override void TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.holdingAncestor)
        {
            // Pick up Ancestor corpse
        }
    }
}
