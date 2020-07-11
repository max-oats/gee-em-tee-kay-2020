using UnityEngine;

public class TowerTypeSelector
{
    public static GameObject TowerToSpawnFromAncestor(Ancestor inAncestor)
    {
        return Game.entities.GetPrefab(EntityType.Tower_Water);
    }
}
