public class Incubator : BaseEntity
{
    private int numEggs = 0;

    public override void TriggerInteract(InteractParams interactParams)
    {
        if (numEggs > 0)
        {
            if (interactParams.holdingWater)
            {
                // Water plant
                interactParams.interactingCharacter.UseWater();
            }
        }
        else
        {
            if (interactParams.eggsToLay > 0)
            {
                // Lay egg
                numEggs = interactParams.eggsToLay;
                interactParams.interactingCharacter.LayEggs();
            }
        }
    }
}
