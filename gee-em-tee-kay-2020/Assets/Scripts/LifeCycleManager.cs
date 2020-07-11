using UnityEngine;
using System.Collections.Generic;

public class LifeCycleManager : MonoBehaviour
{
    private List<Incubator> registeredIncubators = new List<Incubator>();

    public void RegisterIncubator(Incubator newIncubator)
    {
        registeredIncubators.Add(newIncubator);
    }

    public void RegisterCharacterDeath(GameObject oldPlayer)
    {
        Destroy(oldPlayer);

        Vector2Int spawnPosition = new Vector2Int();
        foreach (Incubator incubator in registeredIncubators)
        {
            if (incubator.CanSpawnNewPlayerAt(ref spawnPosition))
            {
                incubator.UseEgg();
                GameObject newPlayer = Game.worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(EntityType.Player), spawnPosition.x, spawnPosition.y).gameObject;
                Game.camera.Follow(newPlayer.transform);
                return;
            }
        }

        Game.gameStateMachine.GameOver();
    }
}
