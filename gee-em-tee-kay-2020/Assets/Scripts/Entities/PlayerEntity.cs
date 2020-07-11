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

    public override void TriggerInteract(InteractParams interactParams) {}

    public void GatherAncestor()
    {
        // Trigger Ancestor
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
