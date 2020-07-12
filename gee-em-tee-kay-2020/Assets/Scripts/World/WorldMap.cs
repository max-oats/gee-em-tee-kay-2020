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

    public void AddInhabitant(int x, int y, BaseEntity newInhabitant)
    {
        AddInhabitant(tileGrid[x,y], newInhabitant);
    }

    public void AddInhabitant(WorldTile tile, BaseEntity newInhabitant)
    {
        tile.AddInhabitant(newInhabitant);
        if (newInhabitant)
        {
            newInhabitant.SetTile(tile);
        }
    }

    public void RemoveInhabitant(int x, int y, BaseEntity inhabitant)
    {
        RemoveInhabitant(tileGrid[x,y], inhabitant);
    }

    public void RemoveInhabitant(WorldTile tile, BaseEntity inhabitant)
    {
        tile.RemoveInhabitant(inhabitant);
    }

    public bool InteractWith(int x, int y, InteractParams interactParams)
    {
        return tileGrid[x,y].TriggerInteract(interactParams);
    }

    public BaseEntity CreateEntityAtLocation(GameObject entityPrefab, WorldTile inTile)
    {
        return CreateEntityAtLocation(entityPrefab, inTile.x, inTile.z);
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
        AddInhabitant(tileX, tileY, entity);
        Game.entities.RegisterNewEntity(entity);
        return entity;
    }

    public Vector2Int? FindAvailableNeighbourTo(WorldTile inTile)
    {
        int posX = inTile.x, posY = inTile.z;

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
