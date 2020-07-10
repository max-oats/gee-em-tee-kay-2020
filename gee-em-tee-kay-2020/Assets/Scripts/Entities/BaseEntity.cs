using UnityEngine;

public class InteractParams
{
    public bool holdingWater = false;
    public bool holdingAncestor = false;
    public bool canLayEgg = false;
}

public abstract class BaseEntity : MonoBehaviour
{
    public abstract void TriggerInteract(InteractParams interactParams);
}
