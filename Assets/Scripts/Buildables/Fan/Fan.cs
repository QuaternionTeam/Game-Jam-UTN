using UnityEngine;

public class Fan : Buildable
{
    [SerializeField] private float range = 5f;
    [SerializeField, Range(1f, 179f)] private float coneAngle = 45f;
    [SerializeField] private float force = 10f;

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.right;
        float halfAngle = coneAngle * 0.5f;

        foreach (Bubble bubble in FindObjectsByType<Bubble>(FindObjectsSortMode.None))
        {
            Vector3 toBubble = bubble.transform.position - origin;
            float distance = toBubble.magnitude;
            if (distance > range || distance <= Mathf.Epsilon)
                continue;
            if (Vector3.Angle(direction, toBubble) > halfAngle)
                continue;

            float falloff = 1f - distance / range;
            bubble.AddForce(direction * (force * falloff));
        }
    }

  void OnEnable()
  {
    GameEvents.RequestPlaySound("Fan");
  }

  void OnDisable()
  {
    GameEvents.RequestStopSound("Fan");
  }

  void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.right;
        float radius = Mathf.Tan(coneAngle * 0.5f * Mathf.Deg2Rad) * range;

        Vector3 up = Mathf.Abs(Vector3.Dot(direction, Vector3.up)) > 0.99f ? Vector3.right : Vector3.up;
        Vector3 right = Vector3.Cross(up, direction).normalized;
        up = Vector3.Cross(direction, right).normalized;

        Vector3 center = origin + direction * range;
        const int segments = 32;
        Vector3 previous = center + right * radius;
        for (int i = 1; i <= segments; i++)
        {
            float angle = i / (float)segments * Mathf.PI * 2f;
            Vector3 point = center + (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(previous, point);
            previous = point;
        }

        for (int i = 0; i < 4; i++)
        {
            float angle = i / 4f * Mathf.PI * 2f;
            Vector3 edge = center + (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(origin, edge);
        }
    }

    public override string Name() => "Fan";
}
