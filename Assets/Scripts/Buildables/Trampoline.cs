using UnityEngine;

public class Trampoline : Buildable
{
    [SerializeField] private float launchSpeed = 8f;

    void OnTriggerEnter(Collider other)
    {
        TryLaunch(other.transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        TryLaunch(collision.transform);
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
    }
}
