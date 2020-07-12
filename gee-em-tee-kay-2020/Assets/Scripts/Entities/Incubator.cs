using UnityEngine;

public class Incubator : BaseEntity
{
    public Color debugColorWithEggs;
    public Color debugColorWhenWatered;

    private int numEggs = 0;
    private bool sufficientlyWatered = false;

    public override bool TriggerInteract(InteractParams interactParams)
    {
        if (numEggs > 0)
        {
            if (interactParams.holdingWater)
            {
                // Water plant
                sufficientlyWatered = true;
                interactParams.interactingCharacter.UseWater();
                return true;
            }
        }
        else if (interactParams.eggsToLay > 0)
        {
            // Lay egg
            numEggs = interactParams.eggsToLay;
            interactParams.interactingCharacter.LayEggs();
            return true;
        }
        return false;
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Incubator;
    }

    public void UseEgg()
    {
        numEggs--;
    }

    public bool CanSpawnNewPlayerAt(ref Vector2Int spawnPosition)
    {
        if (numEggs == 0 || !sufficientlyWatered)
        {
            return false;
        }

        if (Game.worldMap.FindAvailableNeighbourTo(currentWorldTile) is Vector2Int neighbourPosition)
        {
            spawnPosition = neighbourPosition;
            return true;
        }

        return false;
    }

    void Awake()
    {
        Game.lifeCycleManager.RegisterIncubator(this);
    }

    void Update()
    {
        if (numEggs > 0)
        {
            if (sufficientlyWatered)
            {
                debugColor = debugColorWhenWatered;
            }
            else
            {
                debugColor = debugColorWithEggs;
            }
        }
    }
}
