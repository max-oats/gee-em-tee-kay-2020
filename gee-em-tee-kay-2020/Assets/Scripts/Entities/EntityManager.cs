using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityToPrefab
{
    public EntityType type;
    public GameObject prefab;
}

public class EntityManager : MonoBehaviour
{
    public List<EntityToPrefab> entityToPrefabMap = new List<EntityToPrefab>();

    // @robin: list of entities in world too?

    public GameObject GetPrefab(EntityType entityType)
    {
        EntityToPrefab mapping = entityToPrefabMap.Find(x => x.type == entityType);
        if (mapping == null || mapping.prefab == null)
        {
            Debug.LogErrorFormat("No prefab for {0}!", entityType.ToString());
            return null;
        }

        return mapping.prefab;
    }
}
