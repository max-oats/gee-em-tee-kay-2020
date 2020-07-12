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
                RemoveFromMap();
            }
        }

        base.TriggerInteract(interactParams);

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
