public class Hedge : BaseEntity
{
    public override bool TriggerInteract(InteractParams interactParams)
    {
        return false;
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Hedge;
    }
}
