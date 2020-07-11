using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEntity : BaseEntity
{
    public Color debugColourWithWater;
    public Color debugColourWithAncestor;
    public Color debugColourNormal;

    [SerializeField]
    private int maxEggsLaidAtOnce = 1;
    [SerializeField]
    private Vector2Int lifeSpanRange;

    private int currentTimeStepsTillDeath = 0;
    private bool holdingWater = false;
    private Ancestor heldAncestor = null;
    private int eggsToLay = 0;
    private int lifespan = 0;

    //cache
    private Animator animator;
    private PlayerController playerController;

    public override bool TriggerInteract(InteractParams interactParams)
    {
        if (heldAncestor)
        {
            SpawnTowerFrom(heldAncestor, interactParams.tileX, interactParams.tileY);
        }
        else
        {
            // Interacting with yourself is relaxing
            Relax();
        }

        return true;
    }

    public void GatherAncestor(Ancestor inAncestor)
    {
        // Trigger animation
        heldAncestor = inAncestor;
    }

    public void BuryAncestor()
    {
        // Trigger animation
        Game.entities.UnregisterEntity(heldAncestor);
        Destroy(heldAncestor.gameObject);
        heldAncestor = null;
    }

    public void SpawnTowerFrom(Ancestor ancestor, int tileX, int tileY)
    {
        BuryAncestor();

        // Move out of square
        playerController.MoveAside();

        // Spawn tower
        GameObject towerPrefabToSpawn = TowerTypeSelector.TowerToSpawnFromAncestor(ancestor);
        Game.worldMap.CreateEntityAtLocation(towerPrefabToSpawn, tileX, tileY);
    }

    public void UseWater()
    {
        // Trigger animation
        holdingWater = false;
    }

    public void GatherWater()
    {
        // Trigger animation
        holdingWater = true;
    }

    public void LayEggs()
    {
        // Trigger animation
        eggsToLay = 0;
    }

    public void Relax()
    {
        // Trigger animation
        eggsToLay++;
        eggsToLay = Mathf.Min(eggsToLay, maxEggsLaidAtOnce);
    }

    public override void StepTime()
    {
        currentTimeStepsTillDeath--;
        animator.SetFloat("Age", (float)(lifespan-currentTimeStepsTillDeath)/(float)lifespan);
        if (currentTimeStepsTillDeath == 0)
        {
            Game.entities.onTimeStepComplete += Die;
        }
    }

    void Die()
    {
        StartCoroutine(Death());
        Game.entities.onTimeStepComplete -= Die;
    }

    void Awake()
    {
        playerController = GetComponentInChildren<PlayerController>();
        playerController.onInteractWith += InteractWith;
        lifespan = Random.Range(lifeSpanRange.x, lifeSpanRange.y + 1);
        currentTimeStepsTillDeath = lifespan;

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (holdingWater)
        {
            debugColor = debugColourWithWater;
        }
        else if (heldAncestor)
        {
            debugColor = debugColourWithAncestor;
        }
        else
        {
            debugColor = debugColourNormal;
        }
    }

    bool InteractWith(int x, int y)
    {
        InteractParams interactParams = new InteractParams();
        interactParams.holdingWater = holdingWater;
        interactParams.heldAncestor = heldAncestor;
        interactParams.eggsToLay = eggsToLay;
        interactParams.interactingCharacter = this;
        interactParams.tileX = x;
        interactParams.tileY = y;

        return Game.worldMap.InteractWith(x, y, interactParams);
    }

    IEnumerator Death()
    {
        playerController.canMove = false;
        // Trigger animation

        playerController.FaceDirection(Direction.South);
        animator.CrossFadeInFixedTime("Walk", 0.1f);
        yield return new WaitForSeconds(0.1f);
        animator.CrossFadeInFixedTime("Idle", 0.1f);

        yield return new WaitForSeconds(0.5f);


        animator.CrossFadeInFixedTime("Death", 0.1f);

        yield return new WaitForSeconds(3.0f);

        // Unregister entity from entities
        Game.entities.UnregisterEntity(this);

        // Drop held ancestor next to us
        if (heldAncestor)
        {
            if (Game.worldMap.FindAvailableNeighbourTo(posX, posY) is Vector2Int tilePosition)
            {
                Game.worldMap.SetInhabitant(tilePosition.x, tilePosition.y, heldAncestor);
            }
            else
            {
                // Just destroy the ancestor
                Game.entities.UnregisterEntity(heldAncestor);
                Destroy(heldAncestor.gameObject);
            }
            heldAncestor = null;
        }

        // Remove ourselves from square
        Game.worldMap.SetInhabitant(posX, posY, null);

        // Spawn new Ancestor where we are
        Ancestor newAncestor = Game.worldMap.CreateEntityAtLocation(Game.entities.GetPrefab(EntityType.Ancestor), posX, posY) as Ancestor;

        // Reparent player model to ancestor

        // Notify Lifecycle script that we need a new character
        Game.lifeCycleManager.RegisterCharacterDeath(gameObject);
    }
}
