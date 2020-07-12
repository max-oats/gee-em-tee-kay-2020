using UnityEngine;
using System.Collections.Generic;

public class Ancestor : BaseEntity
{
    public static List<EntityType> obstacleTypes = new List<EntityType>();

    [SerializeField]
    private List<EntityType> instanceObstacleTypes = new List<EntityType>();

    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        if (interactParams.interactingType == EntityType.Player)
        {
            PlayerInteractParams playerParams = interactParams as PlayerInteractParams;
            if (!playerParams.holdingWater && !playerParams.heldAncestor)
            {
                interactParams.interactingEntity.InteractionResult(new BaseInteractedWithParams(EntityType.Ancestor, this));
                //interactParams.interactingCharacter.GatherAncestor(this);
                RemoveFromMap();
            }
        }
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Ancestor;
    }

    void Awake()
    {
        obstacleTypes = instanceObstacleTypes;
    }
}
