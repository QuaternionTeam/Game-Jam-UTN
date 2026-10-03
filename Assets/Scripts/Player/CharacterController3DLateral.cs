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

  [Header("Knockback")]
  [SerializeField] private float knockbackDecay = 8f;

  [Header("Grounded Check")]
  [SerializeField] private Transform groundCheck;
  [SerializeField] private float groundCheckRadius = 0.2f;
  [SerializeField] private LayerMask groundLayer;
  [SerializeField] private bool drawGizmos = true;

  [Header("References")]
  [SerializeField] private Transform modelTransform; // Para rotarlo
  [SerializeField] private Transform buildableSpawner;
  [SerializeField] private Animator animator;

  private Rigidbody rigidBody;
  private Vector2 moveInput;
  private Vector3 knockbackVelocity;
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
    InputManager.OnInteractPressed += Build;
  }

  private void OnDisable()
  {
    InputManager.OnJumpPressed -= StartJumpBufferTimer;
    InputManager.OnInteractPressed -= Build;
  }

  private void Build()
  {
    // 1. Validar que exista el LevelManager
    if (LevelManager.Instance == null)
    {
      Debug.LogWarning("No se encontró una instancia de LevelManager en la escena.");
      return;
    }

    BuildableItem itemToBuild = LevelManager.Instance.SelectedBuildable;

    // 2. Validar que el ítem seleccionado sea válido y tenga asignado un prefab
    Debug.Log($"Intentando construir: {itemToBuild?.prefab?.name ?? "Ninguno"}");
    if (itemToBuild == null || itemToBuild.prefab == null)
    {
      Debug.LogWarning("No hay ningún objeto seleccionable válido o el prefab está nulo.");
      return;
    }

    // 3. Verificar si hay stock disponible
    if (itemToBuild.amount > 0)
    {
      Instantiate(itemToBuild.prefab, buildableSpawner.position, buildableSpawner.rotation);

      // 4. Consumir el objeto reduciendo la cantidad
      LevelManager.Instance.RemoveBuildable(itemToBuild.prefab.index);

      Debug.Log($"Objeto {itemToBuild.prefab.name} construido. Quedan: {itemToBuild.amount}");
    }
    else
      Debug.Log($"Sin stock suficiente para construir {itemToBuild.prefab.name}.");
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
    LaunchUpward(jumpForce);

    jumpBufferCounter = 0f;
    coyoteTimeCounter = 0f;

    animator.SetTrigger(JumpHash);
    GameEvents.RequestPlaySound("Boing_Jump");
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

  public void AddKnockback(Vector3 impulse)
  {
    // knockbackVelocity += new Vector3(impulse.x, 0f, impulse.z);
    rigidBody.linearVelocity = new Vector3(0f, impulse.y, 0f);
  }

  public void LaunchUpward(float verticalSpeed)
  {
    rigidBody.linearVelocity = new Vector3(rigidBody.linearVelocity.x, verticalSpeed, rigidBody.linearVelocity.z);
  }

  private void HandleMovement()
  {
    knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, knockbackDecay * Time.fixedDeltaTime);

    // Se aplica el movimiento en el eje X manteniendo la velocidad en Y y Z, más el knockback
    rigidBody.linearVelocity = new Vector3(moveInput.x * moveSpeed + knockbackVelocity.x, rigidBody.linearVelocity.y, rigidBody.linearVelocity.z + knockbackVelocity.z);
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
