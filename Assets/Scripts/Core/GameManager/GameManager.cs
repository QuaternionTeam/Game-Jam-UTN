using UnityEngine;
using UnityEngine.SceneManagement;

internal class GameManager : MonoBehaviour
{
  internal static GameManager Instance = null;
  private readonly FSM<GameState> _gameFSM = new();

  internal void Awake()
  {
    if (Instance != null)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    transform.parent = null;
    DontDestroyOnLoad(gameObject);
  }

  private void Start()
  {
    LoadGameStates();
  }

  private void LoadGameStates()
  {
    _gameFSM.RegisterState(new MainMenuState());
    _gameFSM.RegisterState(new GameplayState());
    _gameFSM.RegisterState(new PauseState());
    _gameFSM.RegisterState(new GameOverState());
    _gameFSM.RegisterState(new VictoryState());

    if (SceneManager.GetActiveScene().name == "MainMenu")
      _gameFSM.ChangeState<MainMenuState>();
    else
      _gameFSM.ChangeState<GameplayState>();
  }

  internal void StartGame()
  {
    _gameFSM.ChangeState<GameplayState>();

    SceneManager.LoadScene(1);
  }

  internal void LoadMainMenu()
  {
    _gameFSM.ChangeState<MainMenuState>();

    SceneManager.LoadScene("MainMenu");
  }

  internal void RetryLevel()
  {
    _gameFSM.ChangeState<GameplayState>();

    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentSceneIndex);
  }

  internal void NextLevel()
  {
    _gameFSM.ChangeState<GameplayState>();

    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    SceneManager.LoadScene(nextSceneIndex);
  }
}
