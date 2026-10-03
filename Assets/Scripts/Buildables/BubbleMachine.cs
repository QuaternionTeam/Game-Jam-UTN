using UnityEngine;

public class BubbleMachine : Buildable
{
    [SerializeField] private Bubble bubblePrefab;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float spawnHeight = 1f;

    void OnEnable()
    {
        InvokeRepeating(nameof(SpawnBubble), spawnInterval, spawnInterval);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void SpawnBubble()
    {
        Instantiate(bubblePrefab, transform.position + Vector3.up * spawnHeight, Quaternion.identity);
    }
}
