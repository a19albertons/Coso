using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]


[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerRump))]
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

    private PlayerMovement playerMovement;
    private Animator animacion;
    private new Rigidbody2D rigidbody;
    private PlayerJump playerJump;
    private BoxCollider2D boxCollider;
    private PlayerRump playerRump;

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

    // Valores originales de los materiales del collider
    private PhysicsMaterial2D originalMaterial;
    private bool botonJumpPressed = false;

    private void Awake()
    {
        // Acciones de movimiento
        gameplayActionMap = inputAction.FindActionMap("Gameplay");
        moveRight = gameplayActionMap.FindAction("MoveRight");
        moveLeft = gameplayActionMap.FindAction("MoveLeft");
        jump = gameplayActionMap.FindAction("Jump");

        // Obtener referencia al componente PlayerMovement y Animator
        playerMovement = GetComponent<PlayerMovement>();
        animacion = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerJump = GetComponent<PlayerJump>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerRump = GetComponent<PlayerRump>();

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
        if (Math.Abs(rigidbody.linearVelocityX) >= 0.1f)
        {
            animacion.SetBool(IsWalkingHash, true);
        }
        else
        {
            animacion.SetBool(IsWalkingHash, false);
        }
        animacion.SetFloat(VelocidadYHash, rigidbody.linearVelocityY);
        animacion.SetBool(EnSueloHash, playerJump.EnSuelo);

        if (rigidbody.linearVelocityX > 0.01f)
        {
            VoltearPersonaje();
        }
        else if (rigidbody.linearVelocityX < -0.01f)
        {
            VoltearPersonaje();
        }
    }

    void FixedUpdate()
    {
        if (playerRump.EnPendiente)
        {
            Vector2 normalMedia = playerRump.NormalMedia();
            if (normalMedia.x < 0f)
            {
                if (moveRight.IsPressed())
                {
                    playerRump.Move(1 / 0.71f * moveSpeedWalking * Vector2.Lerp(Vector2.zero, new Vector2(0.5f, 0.5f), 1f));
                }
                else if (moveLeft.IsPressed())
                {
                    Vector2 normalCentral = playerRump.NormalCentral();
                    if (normalCentral.x < 0f)
                    {
                        playerRump.Move(1 / 0.71f * moveSpeedWalking * Vector2.Lerp(Vector2.zero, new Vector2(-0.5f, -0.5f), 1f));
                    }
                    else
                    {
                        playerMovement.Move(Vector2.Lerp(Vector2.zero, Vector2.left, 1f) * moveSpeedWalking);
                    }
                }
            }
            else if (normalMedia.x > 0f)
            {
                if (moveRight.IsPressed())
                {
                    Vector2 normalCentral = playerRump.NormalCentral();
                    if (normalCentral.x < 0f)
                    {
                        playerRump.Move(1 / 0.71f * moveSpeedWalking * Vector2.Lerp(Vector2.zero, new Vector2(0.5f, -0.5f), 1f));
                    }
                    else
                    {
                        playerMovement.Move(Vector2.Lerp(Vector2.zero, Vector2.right, 1f) * moveSpeedWalking);
                    }
                }
                else if (moveLeft.IsPressed())
                {
                    playerRump.Move(1 / 0.71f * moveSpeedWalking * Vector2.Lerp(Vector2.zero, new Vector2(-0.5f, 0.5f), 1f));
                }
            }
        }
        else
        {
            if (moveRight.IsPressed())
            {
                playerMovement.Move(Vector2.Lerp(Vector2.zero, Vector2.right, 1f) * moveSpeedWalking);
            }
            else if (moveLeft.IsPressed())
            {
                playerMovement.Move(Vector2.Lerp(Vector2.zero, Vector2.left, 1f) * moveSpeedWalking);
            }
        }
        if (jump.IsPressed() && !botonJumpPressed)
        {
            playerJump.Jump(jumpForce);
            botonJumpPressed = true;
        }
        if (!jump.IsPressed())
        {
            botonJumpPressed = false;
        }
        if (playerJump.EnSuelo)
        {
            boxCollider.sharedMaterial = originalMaterial;
        }
        else
        {
            boxCollider.sharedMaterial = materialNoSuelo;
        }
    }

    private void VoltearPersonaje()
    {
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Mathf.Sign(rigidbody.linearVelocityX);
        transform.localScale = escala;
    }
}
