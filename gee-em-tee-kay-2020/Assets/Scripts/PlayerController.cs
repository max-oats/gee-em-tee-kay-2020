using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public delegate void OnMoveTo(int x, int y);
    public OnMoveTo onMoveTo;

    public delegate void OnInteractWith(int x, int y);
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
        if (TryMove(Direction.North, false))
        {
            FaceDirection(Direction.South);
            return;
        }
        if (TryMove(Direction.East, false))
        {
            FaceDirection(Direction.West);
            return;
        }
        if (TryMove(Direction.South, false))
        {
            FaceDirection(Direction.North);
            return;
        }
        
        TryMove(Direction.West, false);
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
            TryMove(Direction.North);
        }
        else if (Game.input.GetButtonDown("Movement.East"))
        {
            TryMove(Direction.East);
        }
        else if (Game.input.GetButtonDown("Movement.South"))
        {
            TryMove(Direction.South);
        }
        else if (Game.input.GetButtonDown("Movement.West"))
        {
            TryMove(Direction.West);
        }
        else if (Game.input.GetButtonDown("Action.Interact"))
        {
            InteractInPlace();
        }
    }

    void InteractInPlace()
    {
        onInteractWith?.Invoke(x, y);
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

    bool TryMove(Direction dir, bool canInteract = true)
    {
        FaceDirection(dir);

        int newX = x;
        int newY = y;

        if (dir == Direction.North)
        {
            newY++;
        }
        else if (dir == Direction.East)
        {
            newX++;
        }
        else if (dir == Direction.South)
        {
            newY--;
        }
        else
        {
            newX--;
        }

        bool success = false;
        if (IsValidLocation(newX, newY))
        {
            if (CanMoveToValidLocation(newX, newY))
            {
                success = true;
                MoveToValidLocation(newX, newY);
                onMoveTo?.Invoke(newX, newY);
            }
            else if (canInteract)
            {
                onInteractWith?.Invoke(newX, newY);
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
