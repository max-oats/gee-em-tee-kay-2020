using UnityEngine;

public class WorldTree : BaseEntity
{
    public Color debugColourWhenPlanted;
    public Color debugColourWhenWatered;

    public override void TriggerInteract(InteractParams interactParams)
    {
        if (interactParams.heldAncestor)
        {
            // Plant ancestor
            debugColor = debugColourWhenPlanted;
            interactParams.interactingCharacter.BuryAncestor();
        }
        else if (interactParams.holdingWater)
        {
            // Water tree
            debugColor = debugColourWhenWatered;
            interactParams.interactingCharacter.UseWater();
        }
    }

    public override void StepTime()
    {
        // Lose health
    }
}
