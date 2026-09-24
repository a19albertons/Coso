using UnityEngine;

public interface IInterfaceJump
{
    Rigidbody2D Rigidbody { get; }

    void Jump(float jumpForce);
}