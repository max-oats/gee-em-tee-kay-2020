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

    [SerializeField]
    private List<BaseEntity> allEntities = new List<BaseEntity>();

    public void RegisterNewEntity(BaseEntity newEntity)
    {
        allEntities.Add(newEntity);
    }

    public void UnregisterEntity(BaseEntity entity)
    {
        allEntities.Remove(entity);
    }

    public void StepTime()
    {
        foreach (BaseEntity entity in allEntities)
        {
            entity.StepTime();
        }
    }

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
