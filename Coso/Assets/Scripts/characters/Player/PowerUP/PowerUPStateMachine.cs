/// <summary>
/// Clase que gestiona la máquina de estados de los powerups del jugador, permitiendo cambiar entre diferentes estados de powerup y ejecutar las acciones correspondientes a cada estado.
/// </summary>
public class PowerUPStateMachine
{
    /// <summary>
    /// Expone el estado actual de la máquina de estados
    /// </summary>
    public PowerUPStates CurrentState { get; private set; }

    /// <summary>
    /// Inicializa la máquina de estados con un estado inicial recibido por parámetro
    /// </summary>
    /// <param name="initial"></param>
    public void Initialize(PowerUPStates initial)
    {
        CurrentState = initial;
        CurrentState.Enter();
    }

    /// <summary>
    /// Gestiona el cambio de estado en la maquina de estados
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(PowerUPStates newState)
    {
        if (newState == null || newState == CurrentState)
        {
            return;
        }
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Jump()
    {
        CurrentState?.Jump();
    }

    public void AplicarCutJump()
    {
        CurrentState?.AplicarCutJump();
    }

    /// <summary>
    /// Devuelve el tipo de powerup que tiene actualmente el jugador
    /// </summary>
    /// <returns></returns>
    public PowerUPType PowerUP()
    {
        return CurrentState.PowerUPType;
    }
}