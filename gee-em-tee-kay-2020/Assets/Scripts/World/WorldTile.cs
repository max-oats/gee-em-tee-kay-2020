using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public int x;
    public int y;

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
        inhabitant.gameObject.transform.position = transform.position;
    }
}
