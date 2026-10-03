using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Player"))
      GameEvents.RequestVictory();
  }
}
