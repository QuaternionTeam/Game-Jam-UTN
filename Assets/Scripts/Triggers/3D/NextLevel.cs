using UnityEngine;

public class NextLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
            GameManager.Instance.NextLevel();
    }
}
