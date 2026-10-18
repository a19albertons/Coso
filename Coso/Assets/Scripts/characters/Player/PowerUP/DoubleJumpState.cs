using UnityEngine;

/// <summary>
/// Clase que gestiona el estado de doble salto del jugador, permitiendo realizar un segundo salto en el aire.
/// </summary>
public class DoubleJumpState : PowerUPStates
{
    public DoubleJumpState(PlayerController player) : base(player) { }

    public override PowerUPType PowerUPType => PowerUPType.DoubleJump;

    /// <summary>
    /// Sobrescribe el método de salto otorgando al jugado la posibilidad de realizar un doble salto
    /// </summary>
    public override void Jump()
    {
        base.Jump();
        if (Player.playerMotor.contadorSaltos < 1)
        {
            Player.rigidbody.linearVelocity = new Vector2(Player.rigidbody.linearVelocity.x, Player.playerMotor.JumpForce);
            Player.playerMotor.contadorSaltos++;
        }
        else if (Player.playerMotor.contadorSaltos == 1)
        {
            Player.rigidbody.linearVelocity = new Vector2(Player.rigidbody.linearVelocity.x, Player.playerMotor.JumpForce * Player.playerMotor.SecondJumpForceMultiplier);
            Player.playerMotor.contadorSaltos++;
        }
    }

    /// <summary>
    /// Sobrescribe el método de corte de salto para permitir que el jugador pueda cortar el salto en el aire si no mantiene presionado el botón de salto, siempre y cuando haya realizado al menos un salto.
    /// </summary>
    public override void AplicarCutJump()
    {
        base.AplicarCutJump();
        if (!Player.JumpState._cutApplied && !Player.inputHandler.JumpHeld && Player.rigidbody.linearVelocity.y > 0.01f && Player.playerMotor.contadorSaltos >= 1)
        {
            Player.playerMotor.CutJump();
            Player.JumpState._cutApplied = true;
        }
    }
}