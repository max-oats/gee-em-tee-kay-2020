abstract class BaseGameState
{
    public abstract void EnterState();
    public abstract void LeaveState();
}

class GameState_Playing : BaseGameState
{
    public override void EnterState()
    {
        // Start World gen etc
    }

    public override void LeaveState()
    {
        // Destroy World etc
    }
}

class GameState_GameOver : BaseGameState
{
    public override void EnterState()
    {
        // Bring up black screen and sad graphic
    }

    public override void LeaveState()
    {
        // hide sad graphic
    }
}
