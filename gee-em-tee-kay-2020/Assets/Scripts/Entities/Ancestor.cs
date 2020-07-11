public class Ancestor : BaseEntity
{
    public int myProperty = 7;
    
    public override void TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.heldAncestor)
        {
            interactParams.interactingCharacter.GatherAncestor(this);
            RemoveFromMap();
        }
    }
}
