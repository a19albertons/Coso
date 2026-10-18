using UnityEngine;

/// <summary>
/// Clase que gestiona el estado de ningún powerup del jugador, permitiendo realizar un solo salto y no tener habilidades especiales.
/// </summary>
public class NoneState : PowerUPStates
{
    public NoneState(PlayerController player) : base(player) { }

    public override PowerUPType PowerUPType => PowerUPType.None;


    public override void Jump()
    {
        base.Jump();
        if (Player.playerMotor.contadorSaltos < 1)
        {
            Player.rigidbody.linearVelocity = new Vector2(Player.rigidbody.linearVelocity.x, Player.playerMotor.JumpForce);
            Player.playerMotor.contadorSaltos++;
        }
    }

    public override void AplicarCutJump()
    {
        base.AplicarCutJump();
        if (!Player.JumpState._cutApplied && !Player.inputHandler.JumpHeld && Player.rigidbody.linearVelocity.y > 0.01f)
        {
            Player.playerMotor.CutJump();
            Player.JumpState._cutApplied = true;
        }
    }
    
}