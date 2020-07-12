// Params passed to the thing being interacted with
public class BaseInteractParams
{
    public BaseInteractParams(EntityType inType, BaseEntity inEntity)
    {
        interactingType = inType;
        interactingEntity = inEntity;
    }
    public int tileX = 0;
    public int tileY = 0;
    public EntityType interactingType;
    public BaseEntity interactingEntity = null;
}

// Params passed back to the thing doing the interacting
public class BaseInteractedWithParams
{
    public BaseInteractedWithParams(EntityType inType, BaseEntity inEntity)
    {
        typeInteractedWith = inType;
        entityInteractedWith = inEntity;
    }

    public EntityType typeInteractedWith;
    public BaseEntity entityInteractedWith = null;
}