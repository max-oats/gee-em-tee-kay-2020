using UnityEngine;
using System.Collections.Generic;

public class EnemyInteractParams : BaseInteractParams
{
    public EnemyInteractParams(EntityType enemyType, BaseEnemy enemy) : base(enemyType, enemy)
    {}

    public int damageDoneToWorldTree = 0;
}

public abstract class BaseEnemy : BaseEntity
{
    [SerializeField]
    private int damageDoneByBullets = 5;
    [SerializeField]
    private int damageDoneByPlayer = 5;
    [SerializeField]
    private int initialHealth = 5;

    [SerializeField]
    private GameObject debugMarkerPrefab = null;
    [SerializeField]
    private bool debugShowPath = false;

    private int currentHealth = 0;

    protected EnemyManager myManager = null;

    protected List<Direction4> currentPath = new List<Direction4>();

    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        switch(interactParams.interactingType)
        {
            case EntityType.Player:
            {
                currentHealth -= damageDoneByPlayer;
                break;
            }
            case EntityType.Bullet:
            {
                currentHealth -= damageDoneByBullets;
                break;
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        base.TriggerInteract(interactParams);    
    }

    public void SetManager(EnemyManager inManager)
    {
        myManager = inManager;
    }
    public void SetPath(List<Direction4> inPath)
    {
        currentPath = new List<Direction4>(inPath);

        // Add Debug
        if (debugMarkerPrefab != null && debugShowPath)
        {
            WorldTile currentTile = currentWorldTile;
            AddDebugMarker(currentTile);
            foreach (Direction4 dir in currentPath)
            {
                currentTile = GetNextTileInDirection(currentTile, dir);
                AddDebugMarker(currentTile);
            }
        }
    }

    protected virtual void Die()
    {
        RemoveFromMap();
        Kill();
    }

    protected WorldTile GetNextTileInDirection(WorldTile currentTile, Direction4 dir)
    {
        switch (dir)
        {
            case Direction4.North:
                return Game.worldMap.GetTileInDirectionFrom(Direction.North, currentTile);
            case Direction4.East:
                return Game.worldMap.GetTileInDirectionFrom(Direction.East, currentTile);
            case Direction4.South:
                return Game.worldMap.GetTileInDirectionFrom(Direction.South, currentTile);
            default:
                return Game.worldMap.GetTileInDirectionFrom(Direction.West, currentTile);
        }
    }

    void Awake()
    {
        currentHealth = initialHealth;
    }

    void AddDebugMarker(WorldTile tile)
    {
        GameObject.Instantiate(debugMarkerPrefab, Game.worldMap.GetTilePos(tile), Quaternion.identity);
    }
}
