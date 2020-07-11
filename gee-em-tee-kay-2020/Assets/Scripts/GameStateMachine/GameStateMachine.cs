using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    private BaseGameState currentState;

    private GameState_Playing gameState_Playing;
    private GameState_GameOver gameState_GameOver;

    public void StartGame()
    {
        TransitionToState(gameState_Playing);
    }

    public void GameOver()
    {
        TransitionToState(gameState_GameOver);
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
    }
}
