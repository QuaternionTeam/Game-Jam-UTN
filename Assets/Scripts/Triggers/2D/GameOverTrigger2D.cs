using UnityEngine;

public class GameOverTrigger2D : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
      GameEvents.RequestGameOver();
  }
}
