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
    public List<EntityToPrefab> entityToPrefabMap = new List<EntityToPrefab>();

    [SerializeField, Socks.Field(category="Debug", readOnly=true)]
    private List<BaseEntity> allEntities = new List<BaseEntity>();

    [SerializeField]
    private int timeStepToBeat = 0;

    private int currentTimeStep = 0;

    public void RegisterNewEntity(BaseEntity newEntity)
    {
        allEntities.Add(newEntity);
    }

    public void UnregisterEntity(BaseEntity entity)
    {
        allEntities.Remove(entity);
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
        foreach (BaseEntity entity in allEntities)
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
}
