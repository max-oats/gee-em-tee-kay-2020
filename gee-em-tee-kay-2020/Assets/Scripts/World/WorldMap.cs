using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public WorldTile[,] tileGrid;
    public int tileSize = 0;
    public Vector3 topLeftCornerLocation = new Vector3();

    public Vector3 GetTilePos(int x, int y)
    {
        return topLeftCornerLocation + new Vector3(x*tileSize, 0f, y*tileSize);
    }

    public bool IsValidLocation(int x, int y)
    {
        // Only works with square map
        bool isValid = (x >= 0 && x < tileGrid.GetLength(0) && y >= 0 && y < tileGrid.GetLength(0));
        return isValid;
    }

    public bool HasInhabitantAt(int x, int y)
    {
        return tileGrid[x,y].HasInhabitant();
    }

    public void SetInhabitant(int x, int y, BaseEntity newInhabitant)
    {
        tileGrid[x,y].SetInhabitant(newInhabitant);
    }

    public void InteractWith(int x, int y, InteractParams interactParams)
    {
        tileGrid[x,y].TriggerInteract(interactParams);
    }
}
