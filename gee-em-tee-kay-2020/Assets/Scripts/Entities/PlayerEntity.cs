using UnityEngine;

public class PlayerEntity : BaseEntity
{
    public int maxEggsLaidAtOnce = 1;

    private bool holdingWater = false;
    private Ancestor heldAncestor = null;
    private int eggsToLay = 0;

    public Color debugColourWithWater;
    public Color debugColourWithAncestor;
    public Color debugColourNormal;

    public override void TriggerInteract(InteractParams interactParams)
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
        Destroy(heldAncestor);
        heldAncestor = null;
    }

    public void SpawnTowerFrom(Ancestor ancestor, int tileX, int tileY)
    {
        BuryAncestor();

        // Move out of square
        PlayerController playerController = GetComponentInChildren<PlayerController>();
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
        // Age?
    }

    void Awake()
    {
        PlayerController playerController = GetComponentInChildren<PlayerController>();
        playerController.onInteractWith += InteractWith;
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

    void InteractWith(int x, int y)
    {
        InteractParams interactParams = new InteractParams();
        interactParams.holdingWater = holdingWater;
        interactParams.heldAncestor = heldAncestor;
        interactParams.eggsToLay = eggsToLay;
        interactParams.interactingCharacter = this;
        interactParams.tileX = x;
        interactParams.tileY = y;

        Game.worldMap.InteractWith(x, y, interactParams);
    }
}
