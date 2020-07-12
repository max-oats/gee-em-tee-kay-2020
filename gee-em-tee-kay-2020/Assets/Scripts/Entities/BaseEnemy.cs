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
    private GameObject debugMarkerPrefab = null;
    [SerializeField]
    private bool debugShowPath = false;

    protected EnemyManager myManager = null;

    protected List<Direction4> currentPath = new List<Direction4>();

    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        // Maybe should implement really weak attack?
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

    void AddDebugMarker(WorldTile tile)
    {
        GameObject.Instantiate(debugMarkerPrefab, Game.worldMap.GetTilePos(tile), Quaternion.identity);
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
}
