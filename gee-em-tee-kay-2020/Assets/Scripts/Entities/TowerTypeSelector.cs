using UnityEngine;

public class TowerTypeSelector
{
    public static GameObject TowerToSpawnFromAncestor(Ancestor inAncestor)
    {
        if (!inAncestor)
        {
            Debug.LogError("Invalid ancestor passed");
        }
        return Game.entities.GetPrefab(EntityType.Tower_Water);
    }
}
