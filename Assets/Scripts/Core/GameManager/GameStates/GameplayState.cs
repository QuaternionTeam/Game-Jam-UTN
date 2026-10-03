using UnityEngine;

public class GameplayState : GameState
{
  public override void OnEnter()
  {
    Time.timeScale = 1f;
    InputManager.Instance.SwitchMap("Player");

    InputManager.OnPausePressed += PauseGame;
    GameEvents.OnVictoryRequested += Victory;
    GameEvents.OnGameOverRequested += GameOver;
  }

  public override void OnExit()
  {
    InputManager.OnPausePressed -= PauseGame;
    GameEvents.OnVictoryRequested -= Victory;
    GameEvents.OnGameOverRequested -= GameOver;
  }

  public override void Update(float deltaTime) { }

  private void PauseGame()
  {
    Transition<PauseState>();
  }

  private void Victory()
  {
    Transition<VictoryState>();
  }

  private void GameOver()
  {
    Transition<GameOverState>();
  }
}
