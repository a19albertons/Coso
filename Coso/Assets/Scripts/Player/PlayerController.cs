using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]


[RequireComponent(typeof(PlayerJump))]
public class PlayerController : MonoBehaviour
{
    // Ligado a controles
    private static readonly int IsWalkingHash = Animator.StringToHash("isMoving");
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

    // Variables de movimiento que estan en el editor
    [SerializeField]
    [Tooltip("Velocidad de movimiento del jugador")]
    private float moveSpeedWalking = 5f;
    [SerializeField]
    [Tooltip("Fuerza de salto del jugador")]
    private float jumpForce = 5f;

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

        if (moveRight.IsPressed())
        {
            playerMovement.Move(Vector2.Lerp(Vector2.zero, Vector2.right, 1f) * moveSpeedWalking);
        }
        else if (moveLeft.IsPressed())
        {
            playerMovement.Move(Vector2.Lerp(Vector2.zero, Vector2.left, 1f) * moveSpeedWalking);
        }
        if (jump.IsPressed())
        {
            playerJump.Jump(jumpForce);
        }
    }

    private void VoltearPersonaje()
    {
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Mathf.Sign(rigidbody.linearVelocityX);
        transform.localScale = escala;
    }
}
