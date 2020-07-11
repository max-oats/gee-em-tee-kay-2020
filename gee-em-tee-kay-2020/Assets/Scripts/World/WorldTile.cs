using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public int x;
    public int z;

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
}
