using UnityEngine;

public class GameOverState : GameState
{
  public override void OnEnter()
  {
    Time.timeScale = 0f;
    InputManager.Instance.SwitchMap("UI");

    GameEvents.TriggerGameOver();
  }

  public override void OnExit()
  {
    Time.timeScale = 1f;
  }

  public override void Update(float deltaTime) { }
}
