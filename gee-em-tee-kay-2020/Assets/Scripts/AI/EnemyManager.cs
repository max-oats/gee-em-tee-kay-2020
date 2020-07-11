using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EntityType[] enemyTypes;
    [SerializeField]
    private AnimationCurve spawnsPerTimeStep = null;

    private int currentTimeStep = 0;
    private int mapSideLengthInTiles = 0;

    void Awake()
    {
        Game.entities.onTimeStepComplete += StepTime;
        mapSideLengthInTiles = GameObject.Find("World").GetComponent<WorldGenerator>().GetSideLengthInTiles();
    }

    void StepTime()
    {
        currentTimeStep++;

        int numSpawns = Mathf.RoundToInt(spawnsPerTimeStep.Evaluate(currentTimeStep));
        if (numSpawns > 0)
        {
            SpawnWaveOf(numSpawns);
        }
    }

    void SpawnWaveOf(int numSpawns)
    {
        // Randomly pick horizontal or vertical side
        //
        // Randomly pick positive or negative of that
        //
        // Randomly pick position along that
        int position = Random.Range(0,mapSideLengthInTiles);

        // Set that as centre of spawning
        Debug.LogFormat("Spawning {0} enemies", numSpawns);
    }
}
