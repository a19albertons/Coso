using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    // Hardcodeo animaciones
    private static readonly int EnSueloHash = Animator.StringToHash("enSuelo");
    private static readonly int VelocidadYHash = Animator.StringToHash("velocidadY");
    private static readonly int IsWalkingHash = Animator.StringToHash("isMoving");

    // Relacionado con controles de movimiento
    [SerializeField]
    private InputActionAsset inputAction;
    private InputActionMap gameplayActionMap;

    private InputAction moveRight;
    private InputAction moveLeft;
    private InputAction jump;

    // Componentes

    private Animator animacion;
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;

    // Variables de movimiento que estan en el editor
    [SerializeField]
    [Tooltip("Velocidad de movimiento del jugador")]
    private float moveSpeedWalking = 5f;

    [SerializeField]
    [Tooltip("Fuerza de salto del jugador")]
    private float jumpForce = 5f;

    [SerializeField]
    [Tooltip("Material del collider cuando el jugador no está en el suelo")]
    private PhysicsMaterial2D materialNoSuelo;

    [SerializeField]
    [Tooltip("Collider que detecta si estamos tocando el suelo o no")]
    private BoxCollider2D posicionDetectorSuelo;

    [SerializeField]
    [Tooltip("Velocidad máxima de movimiento del jugador")]
    private Vector2 maxVelocity = new(2.5f, 0f);
    [SerializeField]
    [Tooltip("Velocidad máxima de movimiento del jugador en la rampa")]
    private Vector2 maxVelocityRump = new(2.5f, 2.5f);

    [SerializeField]
    private LayerMask floorLayer;

    // Valores originales de los materiales del collider
    private PhysicsMaterial2D originalMaterial;
    private bool botonJumpPressed = false;

    // raycast para detectar pendientes
    private RaycastHit2D hit;
    private RaycastHit2D hitIzquierda;
    private RaycastHit2D hitDerecha;

    // Estado del boton
    private bool isMoveRightPressed = false;
    private bool isMoveLeftPressed = false;
    private bool isJumpPressed = false;

    // Gestionan el intervalo de tiempo entre saltos
    private float CooldownJump = 0.5f;
    private float tiempoTranscurrido = 0f;

    public PlayerState playerState {get; private set;}= new();

    private void Awake()
    {
        // Acciones de movimiento
        gameplayActionMap = inputAction.FindActionMap("Gameplay");
        moveRight = gameplayActionMap.FindAction("MoveRight");
        moveLeft = gameplayActionMap.FindAction("MoveLeft");
        jump = gameplayActionMap.FindAction("Jump");

        // Obtener referencia al componente PlayerMovement y Animator
        animacion = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();



        // Guardar el material original del collider
        originalMaterial = boxCollider.sharedMaterial;
    }

    void OnEnable()
    {
        moveRight.Enable();
        moveLeft.Enable();
        jump.Enable();
    }

    void OnDisable()
    {
        moveRight.Disable();
        moveLeft.Disable();
        jump.Disable();
    }

    void Update()
    {
        if (Mathf.Abs(rigidbody.linearVelocityX) >= 0.1f)
        {
            animacion.SetBool(IsWalkingHash, true);
        }
        else
        {
            animacion.SetBool(IsWalkingHash, false);
        }
        animacion.SetFloat(VelocidadYHash, rigidbody.linearVelocityY);
        animacion.SetBool(EnSueloHash, playerState.estadoActual == PlayerState.PlayerStates.Ground || playerState.estadoActual == PlayerState.PlayerStates.Ramp);

        if (rigidbody.linearVelocityX > 0.01f)
        {
            VoltearPersonaje();
        }
        else if (rigidbody.linearVelocityX < -0.01f)
        {
            VoltearPersonaje();
        }

        isMoveRightPressed = moveRight.IsPressed();
        isMoveLeftPressed = moveLeft.IsPressed();
        isJumpPressed = jump.IsPressed();

    }

    void FixedUpdate()
    {
        tiempoTranscurrido += Time.fixedDeltaTime;
        EvaluarEstado();
        if (playerState.estadoActual == PlayerState.PlayerStates.Ramp)
        {
            ColisionadorRampa();
        }
        else
        {
            ColisionadorNormal();
        }

        HandleHorizontalMovement();



        if (isJumpPressed && !botonJumpPressed && playerState.estadoActual == PlayerState.PlayerStates.Ground)
        {
            Jump(jumpForce);
            botonJumpPressed = true;
        }
        if (!isJumpPressed)
        {
            botonJumpPressed = false;
        }
        if (playerState.estadoActual == PlayerState.PlayerStates.Ground)
        {
            boxCollider.sharedMaterial = originalMaterial;
        }
        else
        {
            boxCollider.sharedMaterial = materialNoSuelo;
        }


    }

    private void HandleHorizontalMovement()
    {
        if (playerState.estadoActual == PlayerState.PlayerStates.Ramp)
        {
            Vector2 normalMedia = NormalMedia();
            Vector2 pendiente = new Vector2(normalMedia.y, -normalMedia.x).normalized;

            if (isMoveRightPressed)
            {
                MoveRump(moveSpeedWalking * pendiente);
            }
            // Gestion rampa descendente
            else if (isMoveLeftPressed)
            {
                MoveRump(moveSpeedWalking * -pendiente);
            }
            else
            {
                MoveRump(Vector2.zero);
            }


        }
        else if (playerState.estadoActual == PlayerState.PlayerStates.Ground || playerState.estadoActual == PlayerState.PlayerStates.Jump)
        {
            if (isMoveRightPressed)
            {
                Move(new Vector2(1f, 0f) * moveSpeedWalking);
            }
            else if (isMoveLeftPressed)
            {
                Move(new Vector2(-1f, 0f) * moveSpeedWalking);
            }
            else
            {
                Move(Vector2.zero);
            }
        }
    }

    private void EvaluarEstado()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, floorLayer);
        hitIzquierda = Physics2D.Raycast(transform.position + new Vector3(-0.55f, 0f, 0f), Vector2.down, 0.52f, floorLayer);
        hitDerecha = Physics2D.Raycast(transform.position + new Vector3(0.55f, 0f, 0f), Vector2.down, 0.52f, floorLayer);


        if (Physics2D.OverlapBox(posicionDetectorSuelo.bounds.center, posicionDetectorSuelo.bounds.size * 0.95f, 0f, floorLayer))
        {
            playerState.estadoActual = PlayerState.PlayerStates.Ground;


            // Detecta si estamos en una pendiente a partir de los raycasters
            if ((hit.normal.y != 1.00 && hit.normal.y != 0)
            || (hitIzquierda.normal.y != 1.00 && hitIzquierda.normal.y != 0)
            || (hitDerecha.normal.y != 1.00 && hitDerecha.normal.y != 0)
            )
            {
                playerState.estadoActual = PlayerState.PlayerStates.Ramp;
            }
        }
        else
        {
            playerState.estadoActual = PlayerState.PlayerStates.Jump; 
        }
    }

    private void VoltearPersonaje()
    {
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Mathf.Sign(rigidbody.linearVelocityX);
        transform.localScale = escala;
    }

    private void ColisionadorRampa()
    {
        boxCollider.enabled = false;
        circleCollider.enabled = true;
    }
    private void ColisionadorNormal()
    {
        boxCollider.enabled = true;
        circleCollider.enabled = false;
    }

    private Vector2 NormalMedia()
    {
        Vector2 normalMedia = Vector2.zero;
        int count = 0;
        if (hit.collider != null)
        {
            normalMedia += hit.normal;
            count++;
        }
        if (hitIzquierda.collider != null)
        {
            normalMedia += hitIzquierda.normal;
            count++;
        }
        if (hitDerecha.collider != null)
        {
            normalMedia += hitDerecha.normal;
            count++;
        }
        if (count > 0)
        {
            normalMedia /= count;
            return normalMedia.normalized;
        }
        return Vector2.zero;
    }

    private void Move(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            rigidbody.linearVelocityX = 0f;
            return;
        }
        if (rigidbody.linearVelocity.x + direction.x > maxVelocity.x)
        {
            rigidbody.linearVelocityX = maxVelocity.x;
        }
        else if (rigidbody.linearVelocityX + direction.x < -maxVelocity.x)
        {
            rigidbody.linearVelocityX = -maxVelocity.x;
        }
        else
        {
            rigidbody.AddForce(direction, ForceMode2D.Impulse);
        }
    }

    private void MoveRump(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            rigidbody.linearVelocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            return;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        // Eje X
        if (rigidbody.linearVelocity.x + direction.x > maxVelocityRump.x)
        {
            rigidbody.linearVelocityX = maxVelocityRump.x;

        }
        else if (rigidbody.linearVelocityX + direction.x < -maxVelocityRump.x)
        {
            rigidbody.linearVelocityX = -maxVelocityRump.x;
        }
        else
        {
            rigidbody.AddForce(new Vector2(direction.x, 0f), ForceMode2D.Impulse);
        }


        // Eje Y
        if (rigidbody.linearVelocity.y + direction.y > maxVelocityRump.y)
        {
            rigidbody.linearVelocityY = maxVelocityRump.y;
        }
        else if (rigidbody.linearVelocityY + direction.y < -maxVelocityRump.y)
        {
            rigidbody.linearVelocityY = -maxVelocityRump.y;
        }
        else
        {
            rigidbody.AddForce(new Vector2(0f, direction.y), ForceMode2D.Impulse);
        }
    }

    private void Jump(float jumpForce)
    {
        if (tiempoTranscurrido >= CooldownJump)
        {
            rigidbody.linearVelocityY = 0f;
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            tiempoTranscurrido = 0f;

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * 0.8f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + new Vector3(-0.55f, 0f, 0f), Vector2.down * 0.52f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + new Vector3(0.55f, 0f, 0f), Vector2.down * 0.52f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(posicionDetectorSuelo.bounds.center, posicionDetectorSuelo.bounds.size * 0.95f);
    }


}
