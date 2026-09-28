using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement:  MonoBehaviour, IInterfaceMovement
{
    private Rigidbody2D Rigidbody;
    
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