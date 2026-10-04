using UnityEngine;

public class FallState : PlayerState
{
    public FallState(PlayerController p) : base(p) { }

    public override void Update()
    {
        base.Update();
        PlayerInputHandler input = Player.inputHandler;
        GroundDetector groundDetector = Player.groundDetector;
        if (groundDetector.EnTierra)
        {
            Player.States.ChangeState(
                Mathf.Abs(input.MoveX) > 0.01f ? Player.RunState : Player.IdleState
            );
            return;
        }
        if (input.ConsumeJumpPress())
        {
            Player.States.ChangeState(Player.JumpState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Player.playerMotor.MoveInAir(Player.inputHandler.MoveX);
        Player.playerMotor.ApplyGravity(Player.inputHandler.JumpHeld);
    }
}