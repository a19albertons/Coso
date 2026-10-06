using UnityEngine;

public class PowerUPStateMachine
{
    public PowerUPStates CurrentState { get; private set; }

    public void Initialize(PowerUPStates initial)
    {
        CurrentState = initial;
        CurrentState.Enter();
    }

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
}