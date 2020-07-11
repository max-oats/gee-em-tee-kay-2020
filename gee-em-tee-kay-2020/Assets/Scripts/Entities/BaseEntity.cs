using UnityEngine;

public class InteractParams
{
    public bool holdingWater = false;
    public bool holdingAncestor = false;
    public int eggsToLay = 0;
    public PlayerEntity interactingCharacter = null;
}

public abstract class BaseEntity : MonoBehaviour
{
    public Color debugColor;

    protected int posX;
    protected int posY;

    public abstract void TriggerInteract(InteractParams interactParams);
    public virtual void StepTime() {}

    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    protected void RemoveFromMap()
    {
        Game.worldMap.SetInhabitant(posX, posY, null);
    }
}
