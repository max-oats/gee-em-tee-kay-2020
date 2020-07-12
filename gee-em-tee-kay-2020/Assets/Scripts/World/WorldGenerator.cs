using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DebugEntitySpawnPosition
{
    public EntityType type;
    public Vector2Int position;
}

public class WorldGenerator : MonoBehaviour
{
    [SerializeField, Socks.Field(category="Map")]
    private int sideLengthInTiles = 0;
    [SerializeField, Socks.Field(category="Map")]
    public Vector2Int treeTopLeftCornerLocation = new Vector2Int();
    [SerializeField, Socks.Field(category="Map")]
    private Vector2Int playerInitialPosition = new Vector2Int();

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

    public int GetSideLengthInTiles()
    {
        return sideLengthInTiles;
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

        WorldMap worldMap = null;
        if (Game.worldMap)
        {
            worldMap = Game.worldMap;
        }
        else
        {
            worldMap = FindObjectOfType<WorldMap>();
        }

        worldMap.tileGrid = new WorldTile[sideLengthInTiles,sideLengthInTiles];

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

                newTile.transform.localScale = new Vector3(tileSideLengthInUnits,tileSideLengthInUnits,tileSideLengthInUnits);

                if (i == 0 || i == sideLengthInTiles - 1 || j == 0 || j == sideLengthInTiles - 1)
                {
                    worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(EntityType.Hedge), i,j);
                    continue;
                }

                if (i == treeTopLeftCornerLocation.x || i == treeTopLeftCornerLocation.x + 1)
                {
                    if (j == treeTopLeftCornerLocation.y || j == treeTopLeftCornerLocation.y + 1)
                    {
                        if (i == treeTopLeftCornerLocation.x && j == treeTopLeftCornerLocation.y)
                        {
                            if (worldTreePrefab)
                            {
                                worldTree = worldMap.CreateEntityAtLocation(worldTreePrefab, i, j) as WorldTree;
                            }
                        }
                        else
                        {
                            worldMap.AddInhabitant(i, j, worldTree);
                        }
                    }
                }

                if (i == playerInitialPosition.x && j == playerInitialPosition.y)
                {
                    worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(EntityType.Player), i, j);
                }

                foreach (DebugEntitySpawnPosition debugSpawn in debugEntitySpawnPositionMap)
                {
                    if (debugSpawn.position.x == i && debugSpawn.position.y == j)
                    {
                        worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(debugSpawn.type), i, j);
                    }
                }
            }
        }
    }
}
