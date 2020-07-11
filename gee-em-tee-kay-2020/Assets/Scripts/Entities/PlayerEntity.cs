using UnityEngine;

public class PlayerEntity : BaseEntity
{
    public int maxEggsLaidAtOnce = 1;

    private bool holdingWater = false;
    private bool holdingAncestor = false;
    private int eggsToLay = 0;

    public Color debugColourWithWater;
    public Color debugColourWithAncestor;
    public Color debugColourNormal;

    public override void TriggerInteract(InteractParams interactParams)
    {
        if (holdingAncestor)
        {
            Debug.Log("spawning tower");
            // plant Ancestor, spawn tower
        }
        else
        {
            // Interacting with yourself is relaxing
            Relax();
        }
    }

    public void GatherAncestor()
    {
        // Trigger animation
        holdingAncestor = true;
    }

    public void BuryAncestor()
    {
        // Trigger animation
        holdingAncestor = false;
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
        else if (holdingAncestor)
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
        interactParams.holdingAncestor = holdingAncestor;
        interactParams.eggsToLay = eggsToLay;
        interactParams.interactingCharacter = this;

        Game.worldMap.InteractWith(x, y, interactParams);
    }
}
