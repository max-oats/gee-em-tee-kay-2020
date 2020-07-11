using UnityEngine;

public class InteractParams
{
    public bool holdingWater = false;
    public bool holdingAncestor = false;
    public bool canLayEgg = false;
    public PlayerEntity interactingCharacter = null;
}

public abstract class BaseEntity : MonoBehaviour
{
    public Color debugColor;

    public abstract void TriggerInteract(InteractParams interactParams);
    public virtual void StepTime() {}
}
