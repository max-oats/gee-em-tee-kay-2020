using UnityEngine;

public class WorldTile : MonoBehaviour
{
    public int x = 0;
    public int z = 0;

    private BaseEntity inhabitant = null;

    public bool TriggerInteract(InteractParams interactParams)
    {
        if (inhabitant)
        {
            return inhabitant.TriggerInteract(interactParams);
        }
        return false;
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
        MeshRenderer debugRenderer = GetComponentInChildren<MeshRenderer>();
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_Color", inDebugColour);
        debugRenderer.SetPropertyBlock(block);
    }
}
