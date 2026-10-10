using UnityEngine;

public class DoubleJumpState : PowerUPStates
{
    public DoubleJumpState(PlayerController player) : base(player) { }

    public override PowerUPType PowerUPType => PowerUPType.DoubleJump;

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