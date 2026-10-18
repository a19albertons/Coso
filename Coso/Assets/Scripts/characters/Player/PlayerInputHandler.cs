using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Clase que gestiona las operaciones de input del jugador, incluyendo el movimiento y el salto, así como el buffer de salto.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private float jumpBufferTime = 0.1f; // Tiempo en segundos que se permite el buffer de salto
    // Relacionado con controles de movimiento
    [SerializeField]
    private InputActionAsset inputAction;
    private InputActionMap gameplayActionMap;

    private InputAction moveRight;
    private InputAction moveLeft;
    private InputAction jump;


    // Estado del boton
    private bool isMoveRightPressed = false;
    private bool isMoveLeftPressed = false;
    private bool isJumpPressed = false;
    private float _jumpBufferTime;

    public float MoveX { get; private set; }
    public bool JumpHeld { get; private set; }


    void Awake()
    {
        // Acciones de movimiento
        gameplayActionMap = inputAction.FindActionMap("Gameplay");
        moveRight = gameplayActionMap.FindAction("MoveRight");
        moveLeft = gameplayActionMap.FindAction("MoveLeft");
        jump = gameplayActionMap.FindAction("Jump");
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
        // Actualizamos el estado de los botones de movimiento y salto
        isMoveRightPressed = moveRight.IsPressed();
        isMoveLeftPressed = moveLeft.IsPressed();
        isJumpPressed = jump.IsPressed();

        // Actualizamos el estado en el eje X y el estado del salto
        MoveX = (isMoveRightPressed ? 1f : 0f) + (isMoveLeftPressed ? -1f : 0f);
        JumpHeld = isJumpPressed;

        if (jump.WasPressedThisFrame())
        {
            _jumpBufferTime = jumpBufferTime; // Reiniciamos el tiempo de buffer de salto
        }

        // Actualizamos el tiempo de buffer de salto
        if (_jumpBufferTime > 0)
        {
            _jumpBufferTime -= Time.deltaTime;
        }

    }

    public bool ConsumeJumpPress()
    {
        if (_jumpBufferTime > 0)
        {
            _jumpBufferTime = 0; // Reiniciamos el tiempo de buffer de salto
            return true; // Se consume el salto
        }
        return false; // No se consume el salto
    }

}