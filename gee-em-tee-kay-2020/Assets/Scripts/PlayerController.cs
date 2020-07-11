using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public delegate void OnMoveTo(int x, int y);
    public OnMoveTo onMoveTo;

    public delegate bool OnInteractWith(int x, int y);
    public OnInteractWith onInteractWith;

    [SerializeField, Socks.Field(category="Lifespan")]
    public int startingAge;

    [SerializeField, Socks.Field(category="Lifespan")]
    public Vector2 lifeSpanRange;

    [SerializeField, Socks.Field(category="Debug", readOnly=true)]
    public Direction facing;

    private int x, y;
    private int age;

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
        // stub
    }

    void Update()
    {
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

    void InteractInPlace()
    {
        if (onInteractWith?.Invoke(x, y) ?? false)
        {
            Game.entities.StepTime();
        }
    }

    void FaceDirection(Direction dir)
    {
        if (dir == Direction.North)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
        }
        else if (dir == Direction.East)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
        }
        else if (dir == Direction.South)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270f, transform.eulerAngles.z);
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
            Game.entities.StepTime();
        }
    }

    // Attempts to move somewhere. If we fail and are allowed to interact, do so
    bool TryMove(Direction dir, bool canInteract, out Vector2Int finalPosition)
    {
        FaceDirection(dir);

        finalPosition = new Vector2Int(x,y);

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
        Game.worldMap.SetInhabitant(x,y, null);

        x = newX;
        y = newY;

        transform.position = Game.worldMap.GetTilePos(x, y);
        Game.worldMap.SetInhabitant(x,y, GetComponentInChildren<PlayerEntity>());
    }

    bool CanMoveToValidLocation(int x, int y)
    {
        return !Game.worldMap.HasInhabitantAt(x,y);
    }
}
