using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    private BaseGameState currentState;

    private GameState_Playing gameState_Playing;
    private GameState_GameOver gameState_GameOver;
    private GameState_GameWon gameState_GameWon;

    public void StartGame()
    {
        TransitionToState(gameState_Playing);
    }

    public void GameOver(string reason)
    {
        gameState_GameOver.reason = reason;
        TransitionToState(gameState_GameOver);
    }

    public void WinGame()
    {
        TransitionToState(gameState_GameWon);
    }

    void TransitionToState(BaseGameState newGameState)
    {
        if (currentState != null)
        {
            currentState.LeaveState();
        }

        currentState = newGameState;

        currentState.EnterState();
    }

    void Awake()
    {
        gameState_Playing = new GameState_Playing();
        gameState_GameOver = new GameState_GameOver();
        gameState_GameWon = new GameState_GameWon();

        // Open to main menu?
        StartGame();
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
}
