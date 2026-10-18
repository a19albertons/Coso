using UnityEngine;

/// <summary>
/// Clase que gestiona el estado de salto del jugador, controlando a que estados puede cambiar y las acciones propias de este estado.
/// </summary>
public class JumpState : PlayerState
{
    public bool _cutApplied;
    public JumpState(PlayerController p) : base(p) { }

    public override void Enter()
    {
        _cutApplied = false;
        Player.playerMotor.Jump();
    }

    public override void Update()
    {
        base.Update();

        if (Player.rigidbody.linearVelocity.y < 0f)
        {
            Player.States.ChangeState(Player.FallState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Player.playerMotor.MoveInAir(Player.inputHandler.MoveX);
        Player.playerMotor.ApplyGravity(Player.inputHandler.JumpHeld);
        GestionPowerUP.GetInstance().EstadoPowerUP().AplicarCutJump();
    }

}