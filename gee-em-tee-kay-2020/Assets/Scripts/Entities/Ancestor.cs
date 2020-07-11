public class Ancestor : BaseEntity
{
    public override void TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.heldAncestor)
        {
            interactParams.interactingCharacter.GatherAncestor(this);
            RemoveFromMap();
        }
    }
}
