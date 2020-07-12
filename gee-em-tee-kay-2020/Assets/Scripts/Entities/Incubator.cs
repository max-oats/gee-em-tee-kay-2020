using UnityEngine;

public class IncubatorInteractedWithParams : BaseInteractedWithParams
{
    public IncubatorInteractedWithParams(Incubator inIncubator) : base(EntityType.Incubator, inIncubator)
    { }
    public bool watered = false;
    public bool fertilised = false;
}

public class Incubator : BaseEntity
{
    public Color debugColorWithEggs;
    public Color debugColorWhenWatered;

    private int numEggs = 0;
    private bool sufficientlyWatered = false;

    private Juicer juicer;

    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        if (interactParams.interactingType == EntityType.Player)
        {
            PlayerInteractParams playerParams = interactParams as PlayerInteractParams;
            IncubatorInteractedWithParams returnParams = new IncubatorInteractedWithParams(this);
            if (numEggs > 0)
            {
                if (playerParams.holdingWater)
                {
                    juicer.Stretch(1f, 0.2f);

                    // Water plant
                    sufficientlyWatered = true;
                    returnParams.watered = true;
                    interactParams.interactingEntity.InteractionResult(returnParams);
                }
            }
            else if (playerParams.eggsToLay > 0)
            {
                juicer.Squash(1f, 0.2f);

                // Lay egg
                numEggs = playerParams.eggsToLay;
                returnParams.fertilised = true;
                interactParams.interactingEntity.InteractionResult(returnParams);
            }
        }
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

        if (Game.worldMap.FindAvailableNeighbourTo(currentWorldTile, PlayerEntity.obstacleTypes) is Vector2Int neighbourPosition)
        {
            spawnPosition = neighbourPosition;
            return true;
        }

        return false;
    }

    void Awake()
    {
        Game.lifeCycleManager.RegisterIncubator(this);

        juicer = GetComponent<Juicer>();
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
