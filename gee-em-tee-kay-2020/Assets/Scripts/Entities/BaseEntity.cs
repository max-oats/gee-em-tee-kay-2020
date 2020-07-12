using UnityEngine;
using System.Collections.Generic;

public abstract class BaseEntity : MonoBehaviour
{
    public Color debugColor;

    protected WorldTile currentWorldTile;

    // Return true if the interaction succeeded
    public abstract void TriggerInteract(BaseInteractParams interactParams);
    public virtual void InteractionResult(BaseInteractedWithParams interactedWithParams) {}
    public abstract EntityType GetEntityType();
    public virtual void StepTime() {}

    public virtual void SetTile(WorldTile inTile)
    {
        currentWorldTile = inTile;
    }

    public WorldTile GetTile()
    {
        return currentWorldTile;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(currentWorldTile.x, currentWorldTile.z);
    }

    protected void RemoveFromMap()
    {
        Game.worldMap.RemoveInhabitant(currentWorldTile, this);
    }
}
