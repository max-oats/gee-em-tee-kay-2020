using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EntityType[] enemyTypes = {};
    [SerializeField]
    private AnimationCurve spawnsPerTimeStep = null;
    [SerializeField]
    public EntityList enemyObstacleTypes = null;

    [SerializeField]
    private bool debugSpawn = false;
    [SerializeField]
    private Vector2Int debugSpawnCentre = new Vector2Int();
    [SerializeField]
    private bool debugXIsEdge = false;

    private int mapSideLengthInTiles = 0;
    private AStarPathFinder pathFinder = new AStarPathFinder();
    private List<Vector2Int> possibleGoals = new List<Vector2Int>();
    private List<Vector2Int> availableGoals = new List<Vector2Int>();

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
        pathFinder.Init(new Vector2Int(mapSideLengthInTiles, mapSideLengthInTiles), null);

        foreach (Vector2Int obstaclePosition in Game.worldMap.GetAllObstacleLocations(enemyObstacleTypes.entities))
        {
            pathFinder.PlaceObstacleAt(obstaclePosition);
        }

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
            SpawnEnemy(enemyTypes[Random.Range(0,enemyTypes.Length)], new Vector2Int(x, y));
        }
    }

    public void GetNewPathFor(BaseEnemy enemy, Vector2Int currentPosition)
    {
        // Do this once per step update
        pathFinder.Init(new Vector2Int(mapSideLengthInTiles, mapSideLengthInTiles), ChooseGoal());

        foreach (Vector2Int obstaclePosition in Game.worldMap.GetAllObstacleLocations(enemyObstacleTypes.entities))
        {
            if (obstaclePosition != currentPosition)
            {
                pathFinder.PlaceObstacleAt(obstaclePosition);
            }
        }

        GetPathFor(enemy, currentPosition, null);
    }

    void GetPathFor(BaseEnemy enemy, Vector2Int currentPosition, Vector2Int? goal)
    {
        pathFinder.SetEndPoints(currentPosition, goal);
        List<Direction4> path = pathFinder.GetPath4();
        if (path != null)
        {
            enemy.SetPath(path);
        }
    }

    void SpawnEnemy(EntityType enemyType, Vector2Int spawnPoint)
    {
        BaseEnemy enemy = Game.worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(enemyType), spawnPoint.x, spawnPoint.y) as BaseEnemy;
        enemy.SetManager(this);
        GetPathFor(enemy, spawnPoint, ChooseGoal());
    }

    Vector2Int ChooseGoal()
    {
        if (possibleGoals.Count == 0)
        {
            GetPossibleGoals();
        }

        if (availableGoals.Count == 0)
        {
            availableGoals = new List<Vector2Int>(possibleGoals);
        }

        while (availableGoals.Count > 0)
        {
            Vector2Int goal = availableGoals[0];
            availableGoals.RemoveAt(0);
            if (!Game.worldMap.HasObstacleAt(goal.x, goal.y, enemyObstacleTypes.entities))
            {
                return goal;
            }
        }

        return Game.entities.GetCurrentPlayer().GetPosition();
    }

    void GetPossibleGoals()
    {
        Vector2Int treeTopLeft = GameObject.Find("World").GetComponent<WorldGenerator>().treeBottomLeftCornerLocation;
        Vector2Int currentPoint = treeTopLeft - new Vector2Int(1,1);

        possibleGoals.Add(currentPoint);
        for (int i = 0; i < 3; i++)
        {
            currentPoint += new Vector2Int(1,0);
            possibleGoals.Add(currentPoint);
        }
        for (int i = 0; i < 3; i++)
        {
            currentPoint += new Vector2Int(0,1);
            possibleGoals.Add(currentPoint);
        }
        for (int i = 0; i < 3; i++)
        {
            currentPoint += new Vector2Int(-1,0);
            possibleGoals.Add(currentPoint);
        }
        for (int i = 0; i < 2; i++)
        {
            currentPoint += new Vector2Int(0,-1);
            possibleGoals.Add(currentPoint);
        }

        availableGoals = new List<Vector2Int>(possibleGoals);
    }
}
