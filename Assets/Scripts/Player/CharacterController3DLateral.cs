using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class CharacterController3DLateral : MonoBehaviour
{
  [Header("Movement Settings")]
  [SerializeField] private float moveSpeed = 5f;
  [SerializeField] private float jumpForce = 4f;
  [SerializeField] private float fallMultiplier = 2.5f;
  [SerializeField] private float lowJumpMultiplier = 2f;

  [Header("Feel Settings")]
  [SerializeField] private float coyoteTime = 0.15f;
  [SerializeField] private float jumpBufferTime = 0.15f;

  [Header("Grounded Check")]
  [SerializeField] private Transform groundCheck;
  [SerializeField] private float groundCheckRadius = 0.2f;
  [SerializeField] private LayerMask groundLayer;
  [SerializeField] private bool drawGizmos = true;

  [Header("References")]
  [SerializeField] private Transform modelTransform; // Para rotarlo
  [SerializeField] private Animator animator;

  private Rigidbody rigidBody;
  private Vector2 moveInput;
  private bool isGrounded;

  /* Timers */
  private float coyoteTimeCounter;
  private float jumpBufferCounter;

  /* Hashes de animación */
  private static readonly int SpeedHash = Animator.StringToHash("Speed");
  private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
  private static readonly int VerticalVelocityHash = Animator.StringToHash("VerticalVelocity");
  private static readonly int JumpHash = Animator.StringToHash("Jump");

  private void Awake()
  {
    rigidBody = GetComponent<Rigidbody>();

    // Congelar rotaciones para evitar que el personaje se caiga o gire por colisiones
    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

    if (modelTransform == null)
      modelTransform = transform.Find("Model"); // Intenta asignar el modelo hijo si no está en el inspector

    animator = GetComponentInChildren<Animator>();
  }

  private void Update()
  {
    moveInput = InputManager.Instance.MoveInput;

    CheckGrounded();
    HandleTimers();
    
    TryBufferedJump();
    HandleModelFacing();
    UpdateAnimations();
  }

  private void FixedUpdate()
  {
    HandleMovement();
    ApplyBetterJumpPhysics();
  }

  private void OnEnable()
  {
    InputManager.OnJumpPressed += StartJumpBufferTimer;
  }

  private void OnDisable()
  {
    InputManager.OnJumpPressed -= StartJumpBufferTimer;
  }

  private void StartJumpBufferTimer()
  {
    jumpBufferCounter = jumpBufferTime;
  }

  private void HandleTimers()
  {
    /* Coyote Time */
    if (isGrounded)
      coyoteTimeCounter = coyoteTime;
    else
      coyoteTimeCounter -= Time.deltaTime;

    /* Jump Buffer */
    if (jumpBufferCounter > 0)
      jumpBufferCounter -= Time.deltaTime;
  }

  private void TryBufferedJump()
  {
    if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
      HandleJump();
  }

  private void HandleJump()
  {
    rigidBody.linearVelocity = new Vector3(rigidBody.linearVelocity.x, jumpForce, rigidBody.linearVelocity.z);

    jumpBufferCounter = 0f;
    coyoteTimeCounter = 0f;

    animator.SetTrigger(JumpHash);
  }

  private void ApplyBetterJumpPhysics()
  {
    /* Caída más rápida */
    if (rigidBody.linearVelocity.y < 0)
      rigidBody.linearVelocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
    
    /* Salto corto si NO mantenemos la tecla de salto presionada */
    else if (rigidBody.linearVelocity.y > 0 && !InputManager.Instance.IsJumpHeld)
      rigidBody.linearVelocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime);
  }

  private void HandleMovement()
  {
    // Se aplica el movimiento en el eje X manteniendo la velocidad en Y y Z
    rigidBody.linearVelocity = new Vector3(moveInput.x * moveSpeed, rigidBody.linearVelocity.y, rigidBody.linearVelocity.z);
  }

  private void CheckGrounded()
  {
    if (groundCheck == null) 
      return;

    bool wasGrounded = isGrounded;
    isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

    if (!wasGrounded && isGrounded)
      animator.ResetTrigger(JumpHash);
  }

  private void HandleModelFacing()
  {
    if (modelTransform == null) 
      return;

    /* Rotamos el modelo 3D en el eje Y a 90° o -90° según la dirección */
    if (moveInput.x > 0.1f)
      modelTransform.localRotation = Quaternion.Euler(0f, 90f, 0f);
    else if (moveInput.x < -0.1f)
      modelTransform.localRotation = Quaternion.Euler(0f, -90f, 0f);
  }

  private void UpdateAnimations()
  {
    animator.SetFloat(SpeedHash, Mathf.Abs(moveInput.x));
    animator.SetBool(IsGroundedHash, isGrounded);
    animator.SetFloat(VerticalVelocityHash, rigidBody.linearVelocity.y);
  }

  private void OnDrawGizmosSelected()
  {
    if (!drawGizmos || groundCheck == null)
      return;

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
  }
}
