using UnityEngine;

abstract class BaseGameState
{
    public BaseGameState()
    {
        Init();
    }

    public virtual void Init() {}
    public abstract void EnterState();
    public virtual void Update() {}
    public abstract void LeaveState();
}

class GameState_Playing : BaseGameState
{
    private WorldGenerator worldGenerator = null;

    public override void Init()
    {
        worldGenerator = GameObject.Find("World").GetComponent<WorldGenerator>();
    }

    public override void EnterState()
    {
        if (worldGenerator)
        {
            worldGenerator.GenerateWorld(Game.entities.GetPrefab(EntityType.WorldTree));
        }
    }

    public override void LeaveState()
    {
        if (worldGenerator)
        {
            worldGenerator.ClearWorldTiles();
        }
        Game.entities.ClearAll();
    }
}

class GameState_GameOver : BaseGameState
{
    public string reason = null;

    public override void EnterState()
    {
        // Bring up black screen and sad graphic
        Debug.LogFormat("Game Over: {0}", reason);
    }

    public override void Update()
    {
        if (game.input.GetButtonDown("Restart"))
        {
            game.gameStateMachine.StartGame();
        }
    }

    public override void LeaveState()
    {
        // hide sad graphic
    }
}

class GameState_GameWon : BaseGameState
{
    public override void EnterState()
    {
        // Meteor impact or whatever
        // Bring up black screen and happy graphic
        Debug.Log("You win!");
    }

    public override void LeaveState()
    {
        // hide happy graphic
    }
}
