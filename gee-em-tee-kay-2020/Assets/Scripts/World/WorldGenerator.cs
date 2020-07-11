using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DebugEntitySpawnPosition
{
    public EntityType type;
    public Vector2 position;
}

public class WorldGenerator : MonoBehaviour
{
    [SerializeField, Socks.Field(category="Map")]
    private int sideLengthInTiles = 0;
    [SerializeField, Socks.Field(category="Map")]
    private Vector2 treeTopLeftCornerLocation = new Vector2();
    [SerializeField, Socks.Field(category="Map")]
    private Vector2 playerInitialPosition = new Vector2();

    [SerializeField, Socks.Field(category="Tiles")]
    private int tileSideLengthInUnits = 0;
    [SerializeField, Socks.Field(category="Tiles")]
    private GameObject tilePrefab = null;

    [SerializeField, Socks.Field(category="Debug", readOnly=true)]
    private WorldTree worldTree = null;
    [SerializeField, Socks.Field(category="Debug")]
    private List<DebugEntitySpawnPosition> debugEntitySpawnPositionMap = new List<DebugEntitySpawnPosition>();
    [SerializeField, Socks.Field(category="Debug", readOnly=true)]
    private List<WorldTile> worldTiles = new List<WorldTile>();

    void Awake()
    {
        GenerateWorld(Game.entities.GetPrefab(EntityType.WorldTree));
    }

    public void ClearWorldTiles()
    {
        if (!Application.isPlaying)
        {
            // if not in play mode
            foreach (WorldTile tile in worldTiles)
            {
                if (tile)
                    DestroyImmediate(tile.gameObject);
            }

            if (worldTree)
                DestroyImmediate(worldTree.gameObject);
        }
        else
        {
            foreach (WorldTile tile in worldTiles)
            {
                if (tile)
                    Destroy(tile.gameObject);
            }

            if (worldTree)
                Destroy(worldTree.gameObject);
        }

        worldTree = null;
        worldTiles.Clear();
    }

    public void GenerateWorld(GameObject worldTreePrefab)
    {
        ClearWorldTiles();

        if (worldTreePrefab)
        {
            GameObject worldTreeObject = Instantiate
            (
                worldTreePrefab,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity,
                transform
            ) as GameObject;
            worldTree = worldTreeObject.GetComponent<WorldTree>();
        }

        WorldMap worldMap = null;
        if (Game.worldMap)
        {
            worldMap = Game.worldMap;
        }
        else
        {
            worldMap = FindObjectOfType<WorldMap>();
        }

        if (worldMap)
        {
            worldMap.tileGrid = new WorldTile[sideLengthInTiles,sideLengthInTiles];
        }

        for (int i = 0; i < sideLengthInTiles; i++)
        {
            for (int j = 0; j < sideLengthInTiles; j++)
            {
                int x = i * tileSideLengthInUnits;
                int z = j * tileSideLengthInUnits;

                GameObject newTile = Instantiate
                (
                    tilePrefab,
                    new Vector3(x, 0f, z),
                    Quaternion.identity,
                    transform
                ) as GameObject;

                WorldTile worldTile = newTile.GetComponent<WorldTile>();
                worldTiles.Add(worldTile);
                worldTile.x = i;
                worldTile.z = j;

                if (worldMap)
                {
                    worldMap.tileGrid[i,j] = worldTile;
                }

                newTile.transform.localScale = new Vector3(tileSideLengthInUnits,1f,tileSideLengthInUnits);

                if (i == treeTopLeftCornerLocation.x || i == treeTopLeftCornerLocation.x + 1)
                {
                    if (j == treeTopLeftCornerLocation.y || j == treeTopLeftCornerLocation.y + 1)
                    {
                        worldMap.SetInhabitant(i, j, worldTree);
                    }
                }

                if (i == playerInitialPosition.x && j == playerInitialPosition.y)
                {
                    CreateEntityAtLocation(Game.entities.GetPrefab(EntityType.Player), i, j, x, z);
                }

                foreach (DebugEntitySpawnPosition debugSpawn in debugEntitySpawnPositionMap)
                {
                    if (debugSpawn.position.x == i && debugSpawn.position.y == j)
                    {
                        CreateEntityAtLocation(Game.entities.GetPrefab(debugSpawn.type), i, j, x, z);
                    }
                }
            }
        }
    }

    void CreateEntityAtLocation(GameObject entityPrefab, int tileX, int tileY, int gamePosX, int gamePosZ)
    {
        GameObject newObject = Instantiate
        (
            entityPrefab,
            new Vector3(gamePosX, 0f, gamePosZ),
            Quaternion.identity,
            transform
        ) as GameObject;
        BaseEntity entity = newObject.GetComponent<BaseEntity>();
        Game.worldMap.SetInhabitant(tileX, tileY, entity);
    }
}
