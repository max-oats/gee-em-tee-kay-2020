public class Enemy_WeakMelee : BaseEnemy
{
    public override void StepTime()
    {
        // Do the moving and such
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Enemy_WeakMelee;
    }
}
