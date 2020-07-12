using UnityEngine;

public class InteractParams
{
    public int tileX = 0;
    public int tileY = 0;
    public bool holdingWater = false;
    public Ancestor heldAncestor = null;
    public int eggsToLay = 0;
    public PlayerEntity interactingCharacter = null;
}

public abstract class BaseEntity : MonoBehaviour
{
    public Color debugColor;

    protected WorldTile currentWorldTile;

    // Return true if the interaction succeeded
    public abstract bool TriggerInteract(InteractParams interactParams);
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
