using UnityEngine;

public abstract class Buildable : MonoBehaviour
{
  [SerializeField] private float lifetime = 5f;
  [SerializeField] public int index;

  void Start()
  {
    Destroy(gameObject, lifetime);
  }

  void OnDestroy()
  {
    LevelManager.Instance.StartResourceRecovery(Name());
  }

  abstract public string Name();
}
