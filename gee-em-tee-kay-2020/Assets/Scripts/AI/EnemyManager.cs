using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EntityType[] enemyTypes = {};
    [SerializeField]
    private AnimationCurve spawnsPerTimeStep = null;

    [SerializeField]
    private bool debugSpawn = false;
    [SerializeField]
    private Vector2Int debugSpawnCentre = new Vector2Int();
    [SerializeField]
    private bool debugXIsEdge = false;

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
            int x,y;
            bool xIsEdge;
            if (debugSpawn)
            {
                x = debugSpawnCentre.x;
                y = debugSpawnCentre.y;
                xIsEdge = debugXIsEdge;
            }
            else
            {
                // Randomly pick if we're on positive or negative edge
                int other = Random.value > 0.5 ? 0 : mapSideLengthInTiles-1;
                // Randomly pick position along that edge
                int position = Random.Range(0,mapSideLengthInTiles);
                // Randomly pick whether the edge is horizontal or vertical
                if (Random.value > 0.5)
                {
                    x = position;
                    y = other;
                    xIsEdge = false;
                }
                else
                {
                    x = other;
                    y = position;
                    xIsEdge = true;
                }
            }

            SpawnWave(numSpawns, x, y, xIsEdge);
        }
    }

    void SpawnWave(int numSpawns, int initialX, int initialY, bool xIsEdge)
    {
        for (int i = 0; i < numSpawns; i++)
        {
            int x = initialX, y = initialY;
            if (xIsEdge)
            {
                // Perturb y a bit
                y += Random.value > 0.5 ? i : -i;
                // if y is now off edge, move x instead
                int newY = Mathf.Max(0, Mathf.Min(y, mapSideLengthInTiles-1));
                if (y != newY)
                {
                    int difference = Mathf.Abs(y-newY);
                    y = newY;
                    // Make sure we don't move x off either
                    x += x == 0 ? difference : -difference;
                }
            }
            else
            {
                // Perturb x a bit
                x += Random.value > 0.5 ? i : -i;
                // if x is now off edge, move y instead
                int newX = Mathf.Max(0, Mathf.Min(x, mapSideLengthInTiles-1));
                if (x != newX)
                {
                    int difference = Mathf.Abs(x-newX);
                    x = newX;
                    // Make sure we don't move x off either
                    y += y == 0 ? difference : -difference;
                }
            }
            SpawnEnemy(enemyTypes[Random.Range(0,enemyTypes.Length)], x, y);
        }
    }

    void SpawnEnemy(EntityType enemyType, int x, int y)
    {
        BaseEnemy enemy = Game.worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(enemyType), x, y) as BaseEnemy;
    }
}
