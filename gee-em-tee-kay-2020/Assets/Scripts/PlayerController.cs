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
    }

    bool TryMove(Direction dir)
    {
        int newX = x;
        int newY = y;

        if (dir == Direction.North)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
            newY--;
        }
        else if (dir == Direction.East)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90f, transform.eulerAngles.z);
            newX++;
        }
        else if (dir == Direction.South)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
            newY++;
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270f, transform.eulerAngles.z);
            newX--;
        }

        facing = dir;

        bool success = CanMoveTo(newX, newY);
        if (success)
        {
            MoveTo(newX, newY);
            onMoveTo?.Invoke(newX, newY);
        }
        else
        {
            onInteractWith?.Invoke(newX, newY);
        }

        return success;
    }

    void MoveTo(int newX, int newY)
    {
        Game.worldMap.SetInhabitant(x,y, null);

        x = newX;
        y = newY;

        transform.position = Game.worldMap.GetTilePos(x, y);
        Game.worldMap.SetInhabitant(x,y, GetComponentInChildren<PlayerEntity>());
    }

    bool CanMoveTo(int x, int y)
    {
        return Game.worldMap.IsValidLocation(x,y) && !Game.worldMap.HasInhabitantAt(x,y);
    }
}
