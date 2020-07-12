using UnityEngine;

public class WorldTreeInteractedWithParams : BaseInteractedWithParams
{
    public WorldTreeInteractedWithParams(WorldTree inTree) : base(EntityType.WorldTree, inTree)
    {}

    public bool buriedAncestor = false;
    public bool watered = false;
}

public class WorldTree : BaseEntity
{
    public int initialHealth = 0;
    public int maxHealth = 0;
    public int healthGainedByPlanting = 0;
    public int healthGainedByWatering = 0;
    public int healthGainedPerTimeStep = 0;

    public Color debugColourWhenPlanted;
    public Color debugColourWhenWatered;

    [SerializeField]
    private bool debugLogHealth = false;

    private int currentHealth = 0;
    private Juicer juicer;


    void Awake()
    {
        currentHealth = initialHealth;
        juicer = GetComponent<Juicer>();
    }

    public override void TriggerInteract(BaseInteractParams interactParams)
    {
        switch (interactParams.interactingType)
        {
            case EntityType.Player:
                TriggerInteractByPlayer(interactParams as PlayerInteractParams);
                break;
            case EntityType.Enemy_WeakMelee:
                TriggerInteractByEnemy(interactParams as EnemyInteractParams);
                break;
        }
        base.TriggerInteract(interactParams);
    }

    void TriggerInteractByPlayer(PlayerInteractParams playerParams)
    {
        WorldTreeInteractedWithParams resultParams = new WorldTreeInteractedWithParams(this);
        if (playerParams.heldAncestor)
        {
            // Plant ancestor
            ModifyHealth(healthGainedByPlanting);
            debugColor = debugColourWhenPlanted;
            resultParams.buriedAncestor = true;
            playerParams.interactingEntity.InteractionResult(resultParams);
            juicer.Squash(0.6f, 0.3f);
        }
        else if (playerParams.holdingWater)
        {
            // Water tree
            ModifyHealth(healthGainedByWatering);
            debugColor = debugColourWhenWatered;
            resultParams.watered = true;
            playerParams.interactingEntity.InteractionResult(resultParams);
            juicer.Squash(0.6f, 0.3f);
        }
    }

    void TriggerInteractByEnemy(EnemyInteractParams enemyParams)
    {
        Debug.Log("Tree attacked by enemy!");
        ModifyHealth(-enemyParams.damageDoneToWorldTree);
    }

    public override EntityType GetEntityType()
    {
        return EntityType.WorldTree;
    }

    public override void StepTime()
    {
        // Lose health
        ModifyHealth(healthGainedPerTimeStep);

        if (debugLogHealth)
        {
            Debug.LogFormat("Tree Health {0}", currentHealth);
        }
    }

    public void ModifyHealth(int delta)
    {
        currentHealth += delta;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Game.entities.onTimeStepComplete += Die;
        }
    }

    void Die(int timeStep)
    {
        Game.gameStateMachine.GameOver("World Tree has died");
        Game.entities.onTimeStepComplete -= Die;
    }
}