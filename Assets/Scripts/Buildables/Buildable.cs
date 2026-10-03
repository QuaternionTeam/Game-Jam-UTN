using UnityEngine;

public class Buildable : MonoBehaviour
{
    [SerializeField] private Buildable prefab;
    [SerializeField] private float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnDestroy()
    {
        // GameManager.Instance.Player.Inventory.AddBuildable(prefab);
    }
}
