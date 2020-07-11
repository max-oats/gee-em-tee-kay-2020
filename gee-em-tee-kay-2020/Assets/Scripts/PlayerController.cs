using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public delegate void OnMoveTo(int x, int y);
    public OnMoveTo onMoveTo;

    public delegate bool OnInteractWith(int x, int y);
    public OnInteractWith onInteractWith;

    [SerializeField, Socks.Field(category="Movement")]
    public Smoother rotationSmoother;

    [SerializeField, Socks.Field(category="Movement")]
    public float moveTime = 0.4f;

    [SerializeField, Socks.Field(category="Debug", readOnly=true)]
    public Direction facing;

    [SerializeField, Socks.Field(category="Debug", readOnly=true)]
    public bool canMove = true;

    private Animator animator;
    private PlayerEntity entity;

    public void MoveAside()
    {
        Vector2Int throwawayPosition;
        if (TryMove(Direction.North, false, out throwawayPosition))
        {
            FaceDirection(Direction.South);
            return;
        }
        if (TryMove(Direction.East, false, out throwawayPosition))
        {
            FaceDirection(Direction.West);
            return;
        }
        if (TryMove(Direction.South, false, out throwawayPosition))
        {
            FaceDirection(Direction.North);
            return;
        }

        TryMove(Direction.West, false, out throwawayPosition);
        FaceDirection(Direction.East);
    }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        entity = GetComponentInChildren<PlayerEntity>();
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        if (Game.input.GetButtonDown("Movement.North"))
        {
            PlayerMove(Direction.North);
        }
        else if (Game.input.GetButtonDown("Movement.East"))
        {
            PlayerMove(Direction.East);
        }
        else if (Game.input.GetButtonDown("Movement.South"))
        {
            PlayerMove(Direction.South);
        }
        else if (Game.input.GetButtonDown("Movement.West"))
        {
            PlayerMove(Direction.West);
        }
        else if (Game.input.GetButtonDown("Action.Interact"))
        {
            InteractInPlace();
        }
    }

    void LateUpdate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationSmoother.Smooth(), transform.eulerAngles.z);
    }

    void InteractInPlace()
    {
        Vector2Int currentPosition = entity.GetPosition();
        if (onInteractWith?.Invoke(currentPosition.x, currentPosition.y) ?? false)
        {
            Game.entities.StepTime();
        }
    }

    public void FaceDirection(Direction dir)
    {
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

    // Player intiated move. Will fire onMoveTo callback if successful
    // Will try to interact otherwise
    void PlayerMove(Direction dir)
    {
        Vector2Int positionMovedTo;
        if (TryMove(dir, true, out positionMovedTo))
        {
            onMoveTo?.Invoke(positionMovedTo.x, positionMovedTo.y);
        }
    }

    // Attempts to move somewhere. If we fail and are allowed to interact, do so
    bool TryMove(Direction dir, bool canInteract, out Vector2Int finalPosition)
    {
        FaceDirection(dir);

        finalPosition = entity.GetPosition();

        if (dir == Direction.North)
        {
            finalPosition.y++;
        }
        else if (dir == Direction.East)
        {
            finalPosition.x++;
        }
        else if (dir == Direction.South)
        {
            finalPosition.y--;
        }
        else
        {
            finalPosition.x--;
        }

        bool success = false;
        if (IsValidLocation(finalPosition.x, finalPosition.y))
        {
            if (CanMoveToValidLocation(finalPosition.x, finalPosition.y))
            {
                success = true;
                MoveToValidLocation(finalPosition.x, finalPosition.y);
            }
            else if (canInteract)
            {
                if (onInteractWith?.Invoke(finalPosition.x, finalPosition.y) ?? false)
                {
                    Game.entities.StepTime();
                }
            }
        }

        return success;
    }

    bool IsValidLocation(int x, int y)
    {
        return Game.worldMap.IsValidLocation(x,y);
    }

    void MoveToValidLocation(int newX, int newY)
    {
        Game.worldMap.SetInhabitant(entity.GetTile(), null);

        StartCoroutine(MoveCoroutine(transform.position, Game.worldMap.GetTilePos(newX, newY)));

        Game.worldMap.SetInhabitant(newX,newY, GetComponentInChildren<PlayerEntity>());
    }

    bool CanMoveToValidLocation(int x, int y)
    {
        return !Game.worldMap.HasInhabitantAt(x,y);
    }

    void OnDeath()
    {
        canMove = false;
    }

    IEnumerator MoveCoroutine(Vector3 oldPos, Vector3 newPos)
    {
        canMove = false;
        animator.CrossFadeInFixedTime("Walk", 0.1f);
        float timeCounter = 0f;
        while (timeCounter < moveTime)
        {
            timeCounter += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPos, newPos, timeCounter/moveTime);

            yield return null;
        }

        animator.CrossFadeInFixedTime("Idle", 0.1f);

        canMove = true;
        Game.entities.StepTime();
    }
}
