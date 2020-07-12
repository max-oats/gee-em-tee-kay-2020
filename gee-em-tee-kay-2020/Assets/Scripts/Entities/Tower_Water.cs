using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower_Water : BaseTower
{
    [SerializeField]
    private int cooldownBetweenShots = 0;

    private int timeSinceLastShot = 0;

    public override void StepTime()
    {
        timeSinceLastShot++;

        if (timeSinceLastShot > cooldownBetweenShots)
        {
            timeSinceLastShot = 0;

            // Fire Shots
            FireShotInDirection(Direction.North);
            FireShotInDirection(Direction.East);
            FireShotInDirection(Direction.South);
            FireShotInDirection(Direction.West);
        }
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Tower_Water;
    }

    void Awake()
    {
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        float timeCounter = 0f;
        while (timeCounter < 1f)
        {
            timeCounter += Time.deltaTime;

            currentWorldTile.SetHeight(Mathf.Lerp(0f, 1.5f, timeCounter/1f));

            yield return null;
        }
    }

    void FireShotInDirection(Direction dir)
    {
        WorldTile destination = Game.worldMap.GetTileInDirectionFrom(dir, currentWorldTile);
        if (!destination)
        {
            return;
        }

        if (Game.worldMap.HasObstacleAt(destination, Bullet.obstacleTypes))
        {
            Bullet newBullet = Game.worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(EntityType.Bullet), destination) as Bullet;
            Game.worldMap.InteractWith(destination, new BaseInteractParams(EntityType.Bullet, newBullet));
        }
        else
        {
            Bullet newBullet = Game.worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(EntityType.Bullet), destination) as Bullet;
            newBullet.direction = dir;
        }
    }
}
