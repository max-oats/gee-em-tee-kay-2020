using UnityEngine;

public class Enemy_WeakMelee : BaseEnemy
{
    public int damageDoneToWorldTreePerAttack = 1;
    public override void StepTime()
    {
        // if have a path to move along,
        if (currentPath.Count > 0)
        {
            // try to move
            WorldTile nextTile = GetNextTileInDirection(currentWorldTile, currentPath[0]);
            currentPath.RemoveAt(0);
            if (Game.worldMap.HasObstacleAt(nextTile, myManager.enemyObstacleTypes.entities))
            {
                // cant so ask for new path
                myManager.GetNewPathFor(this, GetPosition());
            }
            else
            {
                Game.worldMap.RemoveInhabitant(currentWorldTile, this);

                // Trigger animation
                //StartCoroutine(MoveCoroutine(transform.position, Game.worldMap.GetTilePos(nextTile)));

                Game.worldMap.AddInhabitant(nextTile, this);
            }
        }
        else 
        {
            // if within space of tree
            WorldTile worldTreeTile = Game.worldMap.FindEntityInNeighbours(EntityType.WorldTree, currentWorldTile);
            if (worldTreeTile)
            {
                // attack
                EnemyInteractParams enemyParams = new EnemyInteractParams(EntityType.Enemy_WeakMelee, this);
                enemyParams.tileX = worldTreeTile.x;
                enemyParams.tileY = worldTreeTile.z;
                enemyParams.damageDoneToWorldTree = damageDoneToWorldTreePerAttack;

                Debug.Log("Enemy attacking!");

                // Trigger attack animation
                Game.worldMap.InteractWith(worldTreeTile, enemyParams);
            }
            else
            {
                // Ask for path
                myManager.GetNewPathFor(this, GetPosition());
            }
        }
    }

    public override EntityType GetEntityType()
    {
        return EntityType.Enemy_WeakMelee;
    }
}
