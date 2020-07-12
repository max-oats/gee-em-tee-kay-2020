using UnityEngine;
using System.Collections.Generic;

public abstract class Bullet : BaseEntity
{
    [SerializeField]
    private List<EntityType> obstacleTypes = new List<EntityType>();

    [System.NonSerialized]
    public Direction direction;

    // When interacted with, damage enemies, get destroyed by bullets and players
    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        if (obstacleTypes.Contains(interactParams.interactingType))
        {
            DestroyWithSplash();
        }

        base.TriggerInteract(interactParams);
    }
    public override void InteractionResult(BaseInteractedWithParams interactedWithParams)
    {
        if (obstacleTypes.Contains(interactedWithParams.typeInteractedWith))
        {
            DestroyWithSplash();
        }
    }
    public override EntityType GetEntityType()
    {
        return EntityType.Bullet;
    }
    public override void StepTime()
    {
        WorldTile destination = Game.worldMap.GetTileInDirectionFrom(direction, currentWorldTile);
        if (!destination)
        {
            DestroyWithSplash();
            return;
        }

        Game.worldMap.RemoveInhabitant(currentWorldTile, this);

        if (Game.worldMap.HasObstacleAt(destination, obstacleTypes))
        {
            // If there, interact
            BaseInteractParams interactParams = new BaseInteractParams(EntityType.Bullet, this);
            interactParams.tileX = destination.x;
            interactParams.tileY = destination.z;
            Game.worldMap.InteractWith(destination, interactParams);
        }
        else
        {
            // Travel animation
            Game.worldMap.AddInhabitant(destination, this);
        }
    }

    void DestroyWithSplash()
    {
        // Trigger Animation

        RemoveFromMap();
    }
}