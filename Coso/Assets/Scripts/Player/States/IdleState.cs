using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController p) : base(p) {}

    public override void Update()
    {
        base.Update();
        PlayerInputHandler input = Player.inputHandler;
        GroundDetector groundDetector = Player.groundDetector;
        if (!groundDetector.EnTierra)
        {
            Player.States.ChangeState(Player.FallState);
            return;
        }
        if (input.ConsumeJumpPress())
        {
            Player.States.ChangeState(Player.JumpState);
            return;
        }
        if (Mathf.Abs(input.MoveX) > 0.1f) 
        {
            Player.States.ChangeState(Player.RunState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Player.playerMotor.MoveOnGround(Player.inputHandler.MoveX, Player.groundDetector.NormalTerreno);
    }
}