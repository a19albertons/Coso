using UnityEngine;

/// <summary>
/// Clase que gestiona la máquina de estados del jugador, permitiendo cambiar entre los diferentes estados y actualizando el estado actual.
/// </summary>
public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState initial)
    {
        CurrentState = initial;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState next)
    {
        if (next == null || next == CurrentState)
        {
            return;
        }
        CurrentState.Exit();
        CurrentState = next;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}