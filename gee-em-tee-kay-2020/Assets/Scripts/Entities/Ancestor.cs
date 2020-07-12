using UnityEngine;
using System.Collections.Generic;

public class Ancestor : BaseEntity
{
    public static List<EntityType> obstacleTypes = new List<EntityType>();

    [SerializeField]
    private List<EntityType> instanceObstacleTypes = new List<EntityType>();

    public override bool TriggerInteract(InteractParams interactParams)
    {
        if (!interactParams.holdingWater && !interactParams.heldAncestor)
        {
            interactParams.interactingCharacter.GatherAncestor(this);
            RemoveFromMap();
            return true;
        }
        return false;
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
