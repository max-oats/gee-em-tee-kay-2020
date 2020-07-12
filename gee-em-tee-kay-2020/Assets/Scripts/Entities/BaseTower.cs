public abstract class BaseTower : BaseEntity
{
    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        // Repair (requires water?)
        base.TriggerInteract(interactParams);
    }
}
