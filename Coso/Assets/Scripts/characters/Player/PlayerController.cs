using UnityEngine;

/// <summary>
/// Clase que gestiona el controlador del jugador, incluyendo la máquina de estados, los componentes y la lógica de actualización.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerVisuals))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    // Componentes
    public new Rigidbody2D rigidbody;

    public GroundDetector groundDetector;
    public PlayerInputHandler inputHandler;
    public PlayerVisuals playerVisuals;
    public PlayerMotor playerMotor;

    public PlayerStateMachine States { get; private set; }

    // Instancias de estados (reutilizables, sin allocs por frame)
    public IdleState IdleState { get; private set; }
    public RunState RunState { get; private set; }
    public JumpState JumpState { get; private set; }
    public FallState FallState { get; private set; }



    // Valores originales de los materiales del collider
    private void Awake()
    {
        // Obtener referencia al componente PlayerMovement y Animator
        rigidbody = GetComponent<Rigidbody2D>();
        groundDetector = GetComponent<GroundDetector>();
        inputHandler = GetComponent<PlayerInputHandler>();
        playerVisuals = GetComponent<PlayerVisuals>();
        playerMotor = GetComponent<PlayerMotor>();

        // State machine
        States = new PlayerStateMachine();
        IdleState = new IdleState(this);
        RunState = new RunState(this);
        JumpState = new JumpState(this);
        FallState = new FallState(this);
    }

    void Start()
    {
        // Iniciar en el estado Idle
        States.Initialize(IdleState);
    }

    void Update()
    {
        playerVisuals.UpdateAnimations(rigidbody.linearVelocity, groundDetector.EnTierra);
        playerVisuals.FaceDirection(inputHandler.MoveX);
        States.Update();
    }

    void FixedUpdate()
    {
        // Actualizar el estado del detector de suelo
        groundDetector.ActualizarEstado();
        States.FixedUpdate();
    }










}
