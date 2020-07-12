using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseEntity : MonoBehaviour
{
    public Color debugColor;

    public Smoother rotationSmoother;

    public Direction facing;

    protected WorldTile currentWorldTile;
    protected bool isActive = true;

    // Return true if the interaction succeeded
    public virtual void TriggerInteract(BaseInteractParams interactParams)
    {
        interactParams.interactingEntity.InteractionResult(new BaseInteractedWithParams(GetEntityType(), this));
    }
    public virtual void InteractionResult(BaseInteractedWithParams interactedWithParams) {}
    public abstract EntityType GetEntityType();
    public virtual void StepTime() {}

    public bool IsActive()
    {
        return isActive;
    }

    public void UpdateRotation()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationSmoother.Smooth(), transform.eulerAngles.z);
    }

    public void Kill()
    {
        isActive = false;
        if (gameObject)
        {
            Destroy(gameObject);
        }
    }

    public virtual void SetTile(WorldTile inTile)
    {
        currentWorldTile = inTile;
    }

    public WorldTile GetTile()
    {
        return currentWorldTile;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(currentWorldTile.x, currentWorldTile.z);
    }

    protected void RemoveFromMap()
    {
        Game.worldMap.RemoveInhabitant(currentWorldTile, this);
    }

    protected IEnumerator MoveCoroutine(Vector3 oldPos, Vector3 newPos)
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator?.CrossFadeInFixedTime("Walk", 0.1f);
        float timeCounter = 0f;
        float moveTime = 0.2f;
        while (timeCounter < moveTime)
        {
            timeCounter += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPos, newPos, timeCounter/moveTime);

            yield return null;
        }

        animator?.CrossFadeInFixedTime("Idle", 0.1f);
    }

    public void FaceDirection(Direction dir)
    {
        rotationSmoother.SetSmoothTime(0.1f);
        rotationSmoother.isAngular = true;
        if (dir == Direction.North)
        {
            rotationSmoother.SetDesiredValue(0f);
        }
        else if (dir == Direction.East)
        {
            rotationSmoother.SetDesiredValue(90f);
        }
        else if (dir == Direction.South)
        {
            rotationSmoother.SetDesiredValue(180f);
        }
        else
        {
            rotationSmoother.SetDesiredValue(270f);
        }

        facing = dir;
    }
}
