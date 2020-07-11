public class Ancestor : BaseEntity
{
    public override void TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.holdingAncestor)
        {
            interactParams.interactingCharacter.GatherAncestor();
            RemoveFromMap();
        }
    }
}
