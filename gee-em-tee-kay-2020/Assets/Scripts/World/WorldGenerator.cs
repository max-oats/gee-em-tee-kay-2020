using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField, Socks.Field(category="Map")]
    private int sideLengthInTiles;
    [SerializeField, Socks.Field(category="Map")]
    private Vector2 treeTopLeftCornerLocation;

    [SerializeField, Socks.Field(category="Tiles")]
    private int tileSideLengthInUnits;
    [SerializeField, Socks.Field(category="Tiles")]
    private GameObject tilePrefab;

    void GenerateWorld()
    {
        for (int x = 0; x < sideLengthInTiles; x++)
        {
            for (int y = 0; y < sideLengthInTiles; y++)
            {
                // Create tiles
                // Handle tree position
            }
        }
    }
}
