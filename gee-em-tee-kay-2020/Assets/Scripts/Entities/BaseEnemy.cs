public class BaseEnemy : BaseEntity
{
    public override bool TriggerInteract(InteractParams interactParams)
    {
        // Maybe should implement really weak attack?
        return false;
    }

    public override void StepTime()
    {
        // Do the moving and such
    }
}
