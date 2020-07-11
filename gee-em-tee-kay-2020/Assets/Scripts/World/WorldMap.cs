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
        if (newInhabitant)
        {
            newInhabitant.SetPosition(x, y);
        }
    }

    public bool InteractWith(int x, int y, InteractParams interactParams)
    {
        return tileGrid[x,y].TriggerInteract(interactParams);
    }

    public BaseEntity CreateEntityAtLocation(GameObject entityPrefab, int tileX, int tileY)
    {
        GameObject newObject = Instantiate
        (
            entityPrefab,
            GetTilePos(tileX, tileY),
            Quaternion.identity,
            transform
        ) as GameObject;
        BaseEntity entity = newObject.GetComponent<BaseEntity>();
        SetInhabitant(tileX, tileY, entity);
        Game.entities.RegisterNewEntity(entity);
        return entity;
    }

    public Vector2Int? FindAvailableNeighbourTo(int posX, int posY)
    {
        int x = posX-1, y = posY;
        if (IsValidLocation(x,y) && !HasInhabitantAt(x,y))
        {
            return new Vector2Int(x,y);
        }

        x = posX+1;
        if (IsValidLocation(x,y) && !HasInhabitantAt(x,y))
        {
            return new Vector2Int(x,y);
        }

        x = posX;
        y = posY-1;
        if (IsValidLocation(x,y) && !HasInhabitantAt(x,y))
        {
            return new Vector2Int(x,y);
        }

        y = posY+1;
        if (IsValidLocation(x,y) && !HasInhabitantAt(x,y))
        {
            return new Vector2Int(x,y);
        }

        return null;
    }
}
