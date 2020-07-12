public class Hedge : BaseEntity
{
    public override void TriggerInteract(BaseInteractParams interactParams)
    {}

    public override EntityType GetEntityType()
    {
        return EntityType.Hedge;
    }
}
