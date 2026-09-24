using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Animator))]


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Ligado a controles
    private static readonly int IsWalkingHash = Animator.StringToHash("isMoving");
    [SerializeField]
    private InputActionAsset inputAction;
    private InputActionMap gameplayActionMap;

    private InputAction moveRight;
    private InputAction moveLeft;

    // Componentes

    private PlayerMovement playerMovement;
    private Animator animacion;
    private new Rigidbody2D rigidbody;

    // Variables de movimiento que estan en el editor
    [SerializeField]
    [Tooltip("Velocidad de movimiento del jugador")]
    private float moveSpeedWalking = 5f;

    private void Awake()
    {
        // Acciones de movimiento
        gameplayActionMap = inputAction.FindActionMap("Gameplay");
        moveRight = gameplayActionMap.FindAction("MoveRight");
        moveLeft = gameplayActionMap.FindAction("MoveLeft");

        // Obtener referencia al componente PlayerMovement y Animator
        playerMovement = GetComponent<PlayerMovement>();
        animacion = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        moveRight.Enable();
        moveLeft.Enable();
    }

    void OnDisable()
    {
        moveRight.Disable();
        moveLeft.Disable();
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
    }

    private void VoltearPersonaje()
    {
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Mathf.Sign(rigidbody.linearVelocityX);
        transform.localScale = escala;
    }
}
