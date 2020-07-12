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
    public delegate void OnTimeStepComplete(int currentStep);
    public OnTimeStepComplete onTimeStepComplete;

    [SerializeField, Socks.Field(category="Entities")]
    private List<EntityToPrefab> entityToPrefabMap = new List<EntityToPrefab>();

    [SerializeField, Socks.Field(category="Entities")]
    private List<EntityType> entityUpdatePriority = new List<EntityType>();

    [SerializeField, Socks.Field(category="Debug", readOnly=true)]
    private List<BaseEntity> allEntities = new List<BaseEntity>();
    [SerializeField, Socks.Field(category="Debug", readOnly=true)]
    private List<BaseEntity> tickingEntities = new List<BaseEntity>();

    [SerializeField]
    private int timeStepToBeat = 0;

    private int currentTimeStep = 0;

    public void RegisterNewEntity(BaseEntity newEntity)
    {
        allEntities.Add(newEntity);

        EntityType newEntityType = newEntity.GetEntityType();
        if (entityUpdatePriority.Contains(newEntityType))
        {
            RegisterTickingEntity(newEntity);
        }
    }

    public void UnregisterEntity(BaseEntity entity)
    {
        allEntities.Remove(entity);
        tickingEntities.Remove(entity);
    }

    public void ClearAll()
    {
        foreach (BaseEntity entity in allEntities)
        {
            Destroy(entity.gameObject);
        }

        allEntities.Clear();
    }

    public void StepTime()
    {
        foreach (BaseEntity entity in tickingEntities)
        {
            entity.StepTime();
        }

        currentTimeStep++;
        if (currentTimeStep < timeStepToBeat)
        {
            onTimeStepComplete?.Invoke(currentTimeStep);
        }
        else
        {
            Game.gameStateMachine.WinGame();
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

    void RegisterTickingEntity(BaseEntity newEntity)
    {
        if (tickingEntities.Count == 0)
        {
            tickingEntities.Add(newEntity);
            return;
        }

        int newEntityPriority = GetEntityPriority(newEntity.GetEntityType());

        int minIndex = 0, maxIndex = tickingEntities.Count;
        while (minIndex != maxIndex)
        {
            // truncated e.g. 3/2 == 1
            int indexToTry = (minIndex + maxIndex) / 2;
            BaseEntity compareTo = tickingEntities[indexToTry];
            int compareToPriority = GetEntityPriority(compareTo.GetEntityType());
            if (newEntityPriority == compareToPriority)
            {
                tickingEntities.Insert(indexToTry, newEntity);
                return;
            }
            else if (newEntityPriority < compareToPriority)
            {
                maxIndex = indexToTry;
            }
            else
            {
                minIndex = indexToTry + 1;
            }
        }
        tickingEntities.Insert(minIndex, newEntity);
    }

    int GetEntityPriority(EntityType inType)
    {
        return entityUpdatePriority.FindIndex(x => x == inType);
    }
}
