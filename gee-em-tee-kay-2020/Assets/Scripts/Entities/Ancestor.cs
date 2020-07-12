public class Ancestor : BaseEntity
{
    public int myProperty = 7;

    public override bool TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.heldAncestor)
        {
            interactParams.interactingCharacter.GatherAncestor(this);
            RemoveFromMap();
            return true;
        }
        return false;
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Ancestor;
    }
}
