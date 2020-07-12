using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ancestor : BaseEntity
{
    public GameObject particleDeath;

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
                StartCoroutine(KillAncestor(playerParams));
            }
        }

        base.TriggerInteract(interactParams);

    }

    IEnumerator KillAncestor(PlayerInteractParams prms)
    {
        PlayerEntity pe = prms.interactingEntity as PlayerEntity;
        PlayerController pc = pe.GetComponent<PlayerController>();
        pc.performingAction = true;
        pc.MoveToValidLocation(GetTile().x, GetTile().z);

        yield return new WaitForSeconds(1.0f);

        pc.FaceDirection(Direction.South);
        pc.GetComponentInChildren<Animator>().CrossFadeInFixedTime("KillAncestor", 0.1f);

        yield return new WaitForSeconds(3f);
        Game.camera.ScreenShake(1.0f);
        GameObject go = Instantiate(particleDeath, transform.position, Quaternion.identity);
        Destroy(go, 5f);
        yield return new WaitForSeconds(0.3f);

        pc.performingAction = false;

        RemoveFromMap();
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
