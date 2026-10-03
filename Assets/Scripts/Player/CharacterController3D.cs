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

  private CharacterController controller;
  private Vector2 moveInput;
  private Vector3 velocity;
  private bool isGrounded;

  // Dirección a la que mira el personaje: derecha (+X) o izquierda (-X)
  private bool facingRight = true;

  /* Hashes de animación */
  private static readonly int SpeedHash = Animator.StringToHash("Speed");
  private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
  private static readonly int JumpHash = Animator.StringToHash("Jump");

  private void Awake()
  {
    controller = GetComponent<CharacterController>();
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
    // Solo se usa el eje X para movimiento lateral estilo plataformero 2D
    float horizontal = moveInput.x;

    if (Mathf.Abs(horizontal) > 0.01f)
    {
      Vector3 moveDirection = new Vector3(horizontal, 0f, 0f);

      // Mover el Character Controller solo en el eje X
      controller.Move(moveDirection * (moveSpeed * Time.deltaTime));

      // Girar solo hacia izquierda o derecha (180° en Y)
      bool movingRight = horizontal > 0f;
      if (movingRight != facingRight)
        facingRight = movingRight;

      Quaternion targetRotation = Quaternion.Euler(0f, facingRight ? 0f : 180f, 0f);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
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

    float currentSpeed = Mathf.Abs(moveInput.x) * moveSpeed;
    // animator.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
    animator.SetFloat(SpeedHash, currentSpeed);
    animator.SetBool(IsGroundedHash, isGrounded);
  }
}
