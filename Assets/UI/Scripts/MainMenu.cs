using UnityEngine;

internal class MainMenu : MonoBehaviour
{
  [SerializeField] private string musicName = "Main_Menu_Music";
  
  private void Start()
  {
    GameEvents.RequestPlaySound(musicName);
  }

  public void StartGame()
  {
    GameManager.Instance.StartGame();
  }
}
