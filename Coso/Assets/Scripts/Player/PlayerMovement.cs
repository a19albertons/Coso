using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement:  MonoBehaviour, IInterfaceMovement
{
    public Rigidbody2D Rigidbody { get; private set; }
    [SerializeField]
    private Vector2 maxVelocity = new(2.5f, 0f);

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        if (Rigidbody.linearVelocity.x + direction.x > maxVelocity.x)
        {
            Rigidbody.linearVelocityX = maxVelocity.x;
        }
        else if (Rigidbody.linearVelocityX + direction.x < -maxVelocity.x)
        {
            Rigidbody.linearVelocityX = -maxVelocity.x;
        }
        else
        {
            Rigidbody.AddForce(direction, ForceMode2D.Impulse);
        }
    }
    
}