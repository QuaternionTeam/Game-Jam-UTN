using UnityEngine;

internal class SpinningBlades : MonoBehaviour
{
  [Header("Configuración de Rotación")]
  [Tooltip("Velocidad de giro en grados por segundo.")]
  [SerializeField] private float rotationSpeed = 90f;

  [SerializeField] private bool rotateClockwise = true;

  private void Update()
  {
    RotateObject();
  }

  private void RotateObject()
  {
    // Determinar la dirección (1 para horario, -1 para antihorario)
    float direction = rotateClockwise ? 1f : -1f;

    // Calculamos la rotación en el eje Y
    float step = rotationSpeed * direction * Time.deltaTime;

    // Aplicamos la rotación en el eje Y sobre el espacio local del objeto
    transform.Rotate(0f, step, 0f, Space.Self);
  }
}