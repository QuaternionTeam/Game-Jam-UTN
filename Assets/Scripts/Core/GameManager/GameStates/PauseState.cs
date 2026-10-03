using UnityEngine;

public class PauseState : GameState
{
  public override void OnEnter()
  {
    Time.timeScale = 0f;
    InputManager.Instance.SwitchMap("UI");

    /* Suscribe */
    GameEvents.OnResumeRequested += ResumeGame;
    InputManager.OnPausePressed += ResumeGame;
    GameEvents.TriggerPause();
  }

  public override void OnExit()
  {
    Time.timeScale = 1f;

    /* Unsuscribe */
    GameEvents.OnResumeRequested -= ResumeGame;
    InputManager.OnPausePressed -= ResumeGame;
    GameEvents.TriggerResume();
  }

  public override void Update(float deltaTime) { }

  private void ResumeGame()
  {
    Transition<GameplayState>();
  }
}
