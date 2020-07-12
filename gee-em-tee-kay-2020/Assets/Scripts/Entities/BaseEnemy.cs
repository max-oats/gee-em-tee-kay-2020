public abstract class BaseEnemy : BaseEntity
{
    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        // Maybe should implement really weak attack?
    }

    public override void StepTime()
    {
        // Do the moving and such
    }
}
