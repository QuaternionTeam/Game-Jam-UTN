using UnityEngine;

public class TrampolineTrigger : MonoBehaviour
{
  [SerializeField] private float launchSpeed = 8f;
  [SerializeField] private Animator animator;
  private static readonly int TriggerHash = Animator.StringToHash("Trigger");

  void OnTriggerEnter(Collider other)
  {
    TryLaunch(other.transform);
  }

  void TryLaunch(Transform other)
  {
    if (!other.CompareTag("Player") && !other.root.CompareTag("Player"))
      return;

    CharacterController3DLateral character = other.GetComponentInParent<CharacterController3DLateral>();
    if (character == null)
      return;

    character.LaunchUpward(launchSpeed);
    GameEvents.RequestPlaySound("Boing_Cartoon");
    animator.SetTrigger(TriggerHash);
  }
}
