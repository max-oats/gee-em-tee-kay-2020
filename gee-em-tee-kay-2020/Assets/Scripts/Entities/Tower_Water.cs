public class Tower_Water : BaseTower
{
    public override void StepTime()
    {
        // Do the shooting of water and such
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Tower_Water;
    }
}
