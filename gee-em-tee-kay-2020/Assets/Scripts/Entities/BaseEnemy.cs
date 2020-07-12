public abstract class BaseEnemy : BaseEntity
{
    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        // Maybe should implement really weak attack?
        base.TriggerInteract(interactParams);    
    }

    public override void StepTime()
    {
        // Do the moving and such
    }
}
