using UnityEngine;

public interface IInterfaceMovement
{
    Rigidbody2D Rigidbody { get; }

    void Move(Vector2 direction);
}
