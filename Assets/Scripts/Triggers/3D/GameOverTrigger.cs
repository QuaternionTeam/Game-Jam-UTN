using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
  private void OnTriggerEnter(Collider collision)
  {
    if (collision.CompareTag("Player"))
      GameEvents.RequestGameOver();
  }
}
