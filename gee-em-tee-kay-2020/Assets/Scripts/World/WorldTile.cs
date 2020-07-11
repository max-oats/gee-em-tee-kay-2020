using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public int x = 0;
    public int z = 0;

    private BaseEntity inhabitant = null;

    public void TriggerInteract(InteractParams interactParams)
    {
        if (inhabitant)
        {
            inhabitant.TriggerInteract(interactParams);
        }
        else if (interactParams.holdingAncestor)
        {
            // plant Ancestor, spawn tower
        }
    }

    public void SetInhabitant(BaseEntity newInhabitant)
    {
        inhabitant = newInhabitant;
    }

    public void SetDebugColour(Color inDebugColour)
    {
        SpriteRenderer debugRenderer = GetComponentInChildren<SpriteRenderer>();
        debugRenderer.color = inDebugColour;
    }
}
