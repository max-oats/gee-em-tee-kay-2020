using UnityEngine;

public class Water : BaseEntity
{
    public Material waterMaterial;

    public override bool TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.heldAncestor)
        {
            interactParams.interactingCharacter.GatherWater();
            return true;
        }
        return false;
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
