public class BaseTower : BaseEntity
{
    public override bool TriggerInteract(InteractParams interactParams)
    {
        // Repair (requires water?)
        return false;
    }
}
