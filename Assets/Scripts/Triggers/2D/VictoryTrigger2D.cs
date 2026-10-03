using UnityEngine;

public class VictoryTrigger2D : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
      GameEvents.RequestVictory();
  }
}
