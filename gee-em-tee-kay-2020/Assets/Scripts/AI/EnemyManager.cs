using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EntityType[] enemyTypes = {};
    [SerializeField]
    private AnimationCurve spawnsPerTimeStep = null;

    private int mapSideLengthInTiles = 0;

    void Awake()
    {
        Game.entities.onTimeStepComplete += StepTime;
        mapSideLengthInTiles = GameObject.Find("World").GetComponent<WorldGenerator>().GetSideLengthInTiles();
    }

    void StepTime(int currentTimeStep)
    {
        int numSpawns = Mathf.RoundToInt(spawnsPerTimeStep.Evaluate(currentTimeStep));
        if (numSpawns > 0)
        {
            // Randomly pick if we're on positive or negative edge
            int other = Random.value > 0.5 ? 0 : mapSideLengthInTiles-1;
            // Randomly pick position along that edge
            int position = Random.Range(0,mapSideLengthInTiles);
            // Randomly pick whether the edge is horizontal or vertical
            int x,y;
            if (Random.value > 0.5)
            {
                x = position;
                y = other;
            }
            else
            {
                x = other;
                y = position;
            }

            SpawnWave(numSpawns, x, y);
        }
    }

    void SpawnWave(int numSpawns, int x, int y)
    {
        for (int i = 0; i < numSpawns; i++)
        {
            SpawnEnemy(enemyTypes[Random.Range(0,enemyTypes.Length)], x, y);
        }
    }

    void SpawnEnemy(EntityType enemyType, int x, int y)
    {
        BaseEnemy enemy = Game.worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(enemyType), x, y) as BaseEnemy;
    }
}
