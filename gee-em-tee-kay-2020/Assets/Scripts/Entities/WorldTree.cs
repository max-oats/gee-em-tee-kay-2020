using UnityEngine;

public class WorldTree : BaseEntity
{
    public int initialHealth = 0;
    public int maxHealth = 0;
    public int healthGainedByPlanting = 0;
    public int healthGainedByWatering = 0;
    public int healthGainedPerTimeStep = 0;

    public Color debugColourWhenPlanted;
    public Color debugColourWhenWatered;

    private int currentHealth = 0;

    void Awake()
    {
        currentHealth = initialHealth;
    }

    public override bool TriggerInteract(InteractParams interactParams)
    {
        if (interactParams.heldAncestor)
        {
            // Plant ancestor
            ModifyHealth(healthGainedByPlanting);
            debugColor = debugColourWhenPlanted;
            interactParams.interactingCharacter.BuryAncestor();
            return true;
        }
        else if (interactParams.holdingWater)
        {
            // Water tree
            ModifyHealth(healthGainedByWatering);
            debugColor = debugColourWhenWatered;
            interactParams.interactingCharacter.UseWater();
            return true;
        }
        return false;
    }

    public override EntityType GetEntityType()
    {
        return EntityType.WorldTree;
    }

    public override void StepTime()
    {
        // Lose health
        ModifyHealth(healthGainedPerTimeStep);

        Debug.LogFormat("Tree Health {0}", currentHealth);
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
        Game.gameStateMachine.GameOver();
        Game.entities.onTimeStepComplete -= Die;
    }
}
