using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField, Socks.Field(category="Map")]
    private int sideLengthInTiles = 0;
    [SerializeField, Socks.Field(category="Map")]
    private Vector2 treeTopLeftCornerLocation = new Vector2();

    [SerializeField, Socks.Field(category="Tiles")]
    private int tileSideLengthInUnits = 0;
    [SerializeField, Socks.Field(category="Tiles")]
    private GameObject tilePrefab = null;

    [SerializeField, Socks.Field(category="Debug")]
    private GameObject worldTreePrefab = null;

    void Awake()
    {
        Debug.Log("Testing generation");
        GenerateWorld(worldTreePrefab);
    }

    void GenerateWorld(GameObject worldTreePrefab)
    {
        WorldTree worldTree = null;

        if (worldTreePrefab)
        {
            GameObject worldTreeObject = Instantiate
            (
                worldTreePrefab,
                new Vector3(0f, 0f, 0f),
                Quaternion.identity
            ) as GameObject;
            worldTree = worldTreeObject.GetComponent<WorldTree>();
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
                    Quaternion.identity
                ) as GameObject;

                WorldTile worldTile = newTile.GetComponent<WorldTile>();
                worldTile.x = i;
                worldTile.z = j;

                newTile.transform.localScale = new Vector3(tileSideLengthInUnits,1f,tileSideLengthInUnits);

                if (i == treeTopLeftCornerLocation.x || i == treeTopLeftCornerLocation.x + 1)
                {
                    if (j == treeTopLeftCornerLocation.y || j == treeTopLeftCornerLocation.y + 1)
                    {
                        worldTile.SetInhabitant(worldTree);
                        worldTree.SetTile(worldTile);
                    }
                }
            }
        }
    }
}
