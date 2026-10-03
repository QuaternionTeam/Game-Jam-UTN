using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 1.5f;
    [SerializeField] private float wobbleSpeed = 2f;
    [SerializeField] private float wobbleAmount = 0.25f;
    [SerializeField] private float windDrag = 2f;
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private float playerBounceForce = 10f;

    private Vector3 windVelocity;
    private float wobblePhase;
    private float previousWobbleX;

    void Start()
    {
        wobblePhase = Random.Range(0f, Mathf.PI * 2f);
        previousWobbleX = CurrentWobbleX();
        Destroy(gameObject, lifetime);
    }

    public void AddForce(Vector3 force)
    {
        windVelocity += force * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.transform.root.CompareTag("Player"))
            return;
        
        BouncePlayer(other.transform, other.ClosestPoint(transform.position));
    }

    void Update()
    {
        windVelocity = Vector3.MoveTowards(windVelocity, Vector3.zero, windDrag * Time.deltaTime);

        float wobbleX = CurrentWobbleX();
        Vector3 position = transform.position;
        position += (Vector3.up * riseSpeed + windVelocity) * Time.deltaTime;
        position.x += wobbleX - previousWobbleX;
        previousWobbleX = wobbleX;
        transform.position = position;
    }

    void BouncePlayer(Transform player, Vector3 contactPoint)
    {
        Vector3 direction = player.position - contactPoint;
        if (direction.sqrMagnitude < 0.0001f)
            direction = player.position - transform.position;
        if (direction.sqrMagnitude < 0.0001f)
            direction = Vector3.up;

        CharacterController3DLateral character = player.GetComponentInParent<CharacterController3DLateral>();
        if (character != null)
            character.AddKnockback(direction.normalized * playerBounceForce);

        Destroy(gameObject);
    }

    float CurrentWobbleX()
    {
        return Mathf.Sin(Time.time * wobbleSpeed + wobblePhase) * wobbleAmount;
    }

    void OnDestroy()
    {
        GameEvents.RequestPlaySound("Bubble_Pop");
    }
}
