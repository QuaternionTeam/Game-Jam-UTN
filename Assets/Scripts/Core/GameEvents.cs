using System;

public static class GameEvents
{
  /* Global Event Requests */
  // public static event Action OnPauseRequested;
  public static event Action OnResumeRequested;
  public static event Action OnGameOverRequested;
  public static event Action OnVictoryRequested;

  /* Global Events */
  public static event Action OnPause;
  public static event Action OnResume;
  public static event Action OnGameOver;
  public static event Action OnVictory;

  /* Audio Events */
  public static Action<string> OnPlaySound;
  public static Action<string> OnStopSound;


  /* Methods to Emit Events*/
  // public static void RequestPause() => OnPauseRequested?.Invoke();
  public static void RequestResume() => OnResumeRequested?.Invoke();
  public static void RequestGameOver() => OnGameOverRequested?.Invoke();
  public static void RequestVictory() => OnVictoryRequested?.Invoke();

  public static void TriggerPause() => OnPause?.Invoke();
  public static void TriggerResume() => OnResume?.Invoke();
  public static void TriggerGameOver() => OnGameOver?.Invoke();
  public static void TriggerVictory() => OnVictory?.Invoke();

  public static void RequestPlaySound(string soundName) => OnPlaySound?.Invoke(soundName);
  public static void RequestStopSound(string soundName) => OnStopSound?.Invoke(soundName);
}
