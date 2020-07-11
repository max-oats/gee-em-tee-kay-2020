public class Water : BaseEntity
{
    public override bool TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.heldAncestor)
        {
            interactParams.interactingCharacter.GatherWater();
            return true;
        }
        return false;
    }

    public override void SetTile(WorldTile inTile)
    {
        base.SetTile(inTile);

        // Change the tile mesh here!
    }
}
