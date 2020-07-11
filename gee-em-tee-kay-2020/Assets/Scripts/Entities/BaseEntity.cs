using UnityEngine;

public class InteractParams
{
    public bool holdingWater = false;
    public bool holdingAncestor = false;
    public bool canLayEgg = false;
    //public Character interactingCharacter = null;
}

public abstract class BaseEntity : MonoBehaviour
{
    public Color debugColor;

    public abstract void TriggerInteract(InteractParams interactParams);
    public virtual void StepTime() {}

    public void SetTile(WorldTile inTile)
    {
        inTile.SetDebugColour(debugColor);
    }
}
