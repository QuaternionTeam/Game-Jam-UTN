using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
      GameEvents.RequestGameOver();
  }
}
