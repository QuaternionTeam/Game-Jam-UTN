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
    LevelManager.Instance.AddBuildable(Name());
    // TODO: Add delay before adding the resource back.
  }

  abstract public string Name();
}
