using UnityEngine;

public class WorldTree : BaseEntity
{
    public Color debugColourWhenPlanted;
    public Color debugColourWhenWatered;

    public override bool TriggerInteract(InteractParams interactParams)
    {
        if (interactParams.heldAncestor)
        {
            // Plant ancestor
            debugColor = debugColourWhenPlanted;
            interactParams.interactingCharacter.BuryAncestor();
            return true;
        }
        else if (interactParams.holdingWater)
        {
            // Water tree
            debugColor = debugColourWhenWatered;
            interactParams.interactingCharacter.UseWater();
            return true;
        }
        return false;
    }

    public override void StepTime()
    {
        // Lose health
    }
}
