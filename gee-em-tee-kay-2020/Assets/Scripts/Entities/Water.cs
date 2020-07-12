using UnityEngine;

public class Water : BaseEntity
{
    public Material waterMaterial;

    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        if (interactParams.interactingType == EntityType.Player)
        {
            PlayerInteractParams playerParams = interactParams as PlayerInteractParams;
            if (!playerParams.holdingWater && !playerParams.heldAncestor)
            {
                base.TriggerInteract(interactParams);
            }
        }
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Water;
    }

    public override void SetTile(WorldTile inTile)
    {
        base.SetTile(inTile);

        inTile.SetMaterial(waterMaterial);
        inTile.SetHeight(-0.1f);
    }
}
