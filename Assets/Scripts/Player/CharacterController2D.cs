using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerController2D : MonoBehaviour
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
  [SerializeField] private SpriteRenderer spriteRenderer;
  [SerializeField] private Animator animator;

  private Rigidbody2D rigidBody;
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
    rigidBody = GetComponent<Rigidbody2D>();

    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    animator = GetComponentInChildren<Animator>();
  }

  private void Update()
  {
    moveInput = InputManager.Instance.MoveInput;

    CheckGrounded();
    HandleTimers();
    
    TryBufferedJump();
    HandleSpriteFlip();
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
    rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);

    jumpBufferCounter = 0f;
    coyoteTimeCounter = 0f;

    animator.SetTrigger(JumpHash);
  }

  private void ApplyBetterJumpPhysics()
  {
    /* Caída mas rápida */
    if (rigidBody.linearVelocity.y < 0)
      rigidBody.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
    
    /* Salto corto si NO mantenemos la tecla de salto presionada */
    else if (rigidBody.linearVelocity.y > 0 && !InputManager.Instance.IsJumpHeld)
      rigidBody.linearVelocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime);
  }

  private void HandleMovement()
  {
    rigidBody.linearVelocity = new Vector2(moveInput.x * moveSpeed, rigidBody.linearVelocity.y);
  }

  private void CheckGrounded()
  {
    if (groundCheck == null) 
      return;

    bool wasGrounded = isGrounded;
    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    if (!wasGrounded && isGrounded)
      animator.ResetTrigger(JumpHash);
  }

  private void HandleSpriteFlip()
  {
    if (moveInput.x > 0.1f)
      spriteRenderer.flipX = false;
    else if (moveInput.x < -0.1f)
      spriteRenderer.flipX = true;
  }

  private void UpdateAnimations()
  {
    animator.SetFloat(SpeedHash, Mathf.Abs(moveInput.x));
    animator.SetBool(IsGroundedHash, isGrounded);
    animator.SetFloat(VerticalVelocityHash, rigidBody.linearVelocity.y);
  }

  private void OnDrawGizmosSelected()
  {
    if (!drawGizmos)
      return;

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
  }
}
