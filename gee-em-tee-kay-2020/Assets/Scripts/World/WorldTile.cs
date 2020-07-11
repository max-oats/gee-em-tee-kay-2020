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
    }

    public bool HasInhabitant()
    {
        return inhabitant != null;
    }

    public void SetInhabitant(BaseEntity newInhabitant)
    {
        inhabitant = newInhabitant;
    }

    void Update()
    {
        if (inhabitant)
        {
            SetDebugColour(inhabitant.debugColor);
        }
        else
        {
            SetDebugColour(Color.white);
        }
    }

    public void SetDebugColour(Color inDebugColour)
    {
        SpriteRenderer debugRenderer = GetComponentInChildren<SpriteRenderer>();
        debugRenderer.color = inDebugColour;
    }
}
