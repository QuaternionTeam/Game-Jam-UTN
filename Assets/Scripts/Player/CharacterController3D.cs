using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterController3D : MonoBehaviour
{
  [Header("Movement Settings")]
  [SerializeField] private float moveSpeed = 5f;
  [SerializeField] private float rotationSpeed = 15f;
  [SerializeField] private float jumpHeight = 1.5f;
  [SerializeField] private float gravity = -19.62f;

  [Header("References")]
  [SerializeField] private Animator animator;
  [SerializeField] private Transform cameraTransform;

  private CharacterController controller;
  private Vector2 moveInput;
  private Vector3 velocity;
  private bool isGrounded;

  /* Hashes de animación */
  private static readonly int SpeedHash = Animator.StringToHash("Speed");
  private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
  private static readonly int JumpHash = Animator.StringToHash("Jump");

  private void Awake()
  {
    controller = GetComponent<CharacterController>();
    
    if (cameraTransform == null && Camera.main != null)
      cameraTransform = Camera.main.transform;
  }

  private void Update()
  {
    moveInput = InputManager.Instance.MoveInput;

    HandleGravity();
    HandleMovement();
    UpdateAnimations();
  }

  private void OnEnable()
  {
    InputManager.OnJumpPressed += HandleJump;
  }

  private void OnDisable()
  {
    InputManager.OnJumpPressed -= HandleJump;
  }

  public void HandleJump()
  {
    if (!isGrounded)
      return;

    // Fórmula física v = sqrt(h * -2 * g)
    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    
    animator.SetTrigger(JumpHash);
  }

  private void HandleMovement()
  {
    if (moveInput.sqrMagnitude < 0.01f)
      return;

    // Calcular dirección de movimiento orientada a la vista de la cámara
    Vector3 forward = cameraTransform.forward;
    Vector3 right = cameraTransform.right;
    
    forward.y = 0f;
    right.y = 0f;
    forward.Normalize();
    right.Normalize();

    Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

    // Mover el Character Controller
    controller.Move(moveDirection * (moveSpeed * Time.deltaTime));

    // Rotar suavemente hacia la dirección del movimiento
    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
  }

  private void HandleGravity()
  {
    isGrounded = controller.isGrounded;

    if (isGrounded && velocity.y < 0)
      velocity.y = -2f; // Pequeño empuje hacia abajo para estabilizar el grounded

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
  }

  private void UpdateAnimations()
  {
    if (animator == null)
      return;

    float currentSpeed = moveInput.magnitude * moveSpeed;
    // animator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
    animator.SetFloat(SpeedHash, currentSpeed);
    animator.SetBool(IsGroundedHash, isGrounded);
  }
}
