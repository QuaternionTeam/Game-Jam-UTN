using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
  private void OnTriggerEnter(Collider collision)
  {
    if (collision.CompareTag("Player"))
      GameEvents.RequestVictory();
  }
}
