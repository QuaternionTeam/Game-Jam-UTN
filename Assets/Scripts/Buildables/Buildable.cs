using UnityEngine;

public class Buildable : MonoBehaviour
{
  [SerializeField] private float lifetime = 5f;
  [SerializeField] public int index;

  void Start()
  {
    Destroy(gameObject, lifetime);
  }

  void OnDestroy()
  {
    LevelManager.Instance.AddBuildable(index);
    // TODO: Add delay before adding the resource back.
  }
}
