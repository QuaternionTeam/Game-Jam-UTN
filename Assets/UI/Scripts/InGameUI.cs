using UnityEngine;

internal class InGameUI : MonoBehaviour
{
  [Header("Panels")]
  [SerializeField] private GameObject pausePanel;
  [SerializeField] private GameObject settingsPanel;
  [SerializeField] private GameObject gameOverPanel;
  [SerializeField] private GameObject victoryPanel;

  private void OnEnable()
  {
    GameEvents.OnPause += ShowPauseMenu;
    GameEvents.OnResume += HideAllPanels;
    GameEvents.OnGameOver += ShowGameOverMenu;
    GameEvents.OnVictory += ShowVictoryMenu;
  }

  private void OnDisable()
  {
    GameEvents.OnPause -= ShowPauseMenu;
    GameEvents.OnResume -= HideAllPanels;
    GameEvents.OnGameOver -= ShowGameOverMenu;
    GameEvents.OnVictory -= ShowVictoryMenu;
  }

  private void ShowPauseMenu()
  {
    HideAllPanels();
    pausePanel.SetActive(true);
  }

  private void ShowGameOverMenu()
  {
    HideAllPanels();
    gameOverPanel.SetActive(true);
  }

  private void ShowVictoryMenu()
  {
    HideAllPanels();
    victoryPanel.SetActive(true);
  }

  public void HideAllPanels()
  {
    pausePanel.SetActive(false);
    settingsPanel.SetActive(false);
    gameOverPanel.SetActive(false);
    victoryPanel.SetActive(false);
  }

  public void Resume()
  {
    GameEvents.RequestResume();
  }

  public void MainMenu()
  {
    GameManager.Instance.LoadMainMenu();
  }

  public void Retry()
  {
    GameManager.Instance.RetryLevel();
  }
}
