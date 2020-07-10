public class Incubator : BaseEntity
{
    private bool isFertilised = false;

    public override void TriggerInteract(InteractParams interactParams)
    {
        if (isFertilised)
        {
            if (interactParams.holdingWater)
            {
                // Water plant
            }
        }
        else
        {
            if (interactParams.canLayEgg)
            {
                // Lay egg
                isFertilised = true;
            }
        }
    }
}
