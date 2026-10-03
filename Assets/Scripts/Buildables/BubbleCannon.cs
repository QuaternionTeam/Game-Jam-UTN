using UnityEngine;

public class BubbleCannon : Buildable
{
    [SerializeField] internal Bubble bubblePrefab;
    [SerializeField] private Animator animator;
    [SerializeField] internal Transform bubbleSpawner;
    [SerializeField] private float spawnInterval = 3f;
    
    private static readonly int TriggerHash = Animator.StringToHash("Trigger");

    void OnEnable()
    {
        InvokeRepeating(nameof(Shoot), spawnInterval, spawnInterval);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Shoot()
    {
      animator.SetTrigger(TriggerHash);
    }
}
