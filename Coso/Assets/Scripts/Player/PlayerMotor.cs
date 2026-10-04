using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]


public class PlayerMotor : MonoBehaviour
{
    [Header("Movimiento Eje X")]
    [SerializeField]
    [Tooltip("Velocidad de movimiento del jugador")]
    private float moveSpeedWalking = 5f;
    [SerializeField]
    [Tooltip("Aceleración del jugador")]
    private float acceleration = 10f;
    [SerializeField]
    [Tooltip("Desaceleración del jugador")]
    private float desacceleration = 10f;
    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Factor de control en el aire")]
    private float airControlFactor = 0.5f;

    [Header("Salto")]
    [SerializeField]
    [Tooltip("Fuerza de salto del jugador")]
    private float jumpForce = 5f;
    [SerializeField]
    [Tooltip("Multiplicador de fuerza segundo salto")]
    private float secondJumpForceMultiplier = 0.5f;
    [SerializeField]
    [Tooltip("gravedad del jugador")]
    private float gravity = 9.81f;
    [SerializeField]
    [Tooltip("Multiplicador de gravedad cuando el jugador cae")]
    private float fallMultiplier = 1f;
    [SerializeField]
    [Tooltip("Multiplicador de gravedad cuando el jugador salta")]
    private float lowJumpMultiplier = 1f;
    [SerializeField]
    [Tooltip("Maxima velocidad de caida del jugador")]
    private float maxFallSpeed = 20f;

    // Componentes
    private Rigidbody2D rb;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;       // gravedad manual
        rb.freezeRotation = true;

    }


    public void MoveOnGround(float direction, Vector2 groundNormal)
    {
        if (groundNormal.sqrMagnitude < 0.000001f)
        {
            groundNormal = Vector2.up;
        }

        Vector2 tangent = Vector2.Perpendicular(groundNormal).normalized;
        if (tangent.x < 0f)
        {
            tangent = -tangent;
        }

        Vector2 targetVelocity = tangent * (direction * moveSpeedWalking);
        float accel = Mathf.Abs(direction) > 0.01f ? acceleration : desacceleration;
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, accel * Time.fixedDeltaTime);
    }

    public void MoveInAir(float direction)
    {
        float targetVelocityX = direction * moveSpeedWalking;
        float newVx = Mathf.MoveTowards(rb.linearVelocity.x, targetVelocityX, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newVx, rb.linearVelocity.y);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public void ApplyGravity(bool JumpHeld)
    {
        float g = gravity;
        if (rb.linearVelocityY < -0.01f)
        {
            g *= fallMultiplier;
        }
        else if (rb.linearVelocityY > 0.01f && !JumpHeld)
        {
            g *= lowJumpMultiplier;
        }

        float newVy = rb.linearVelocityY - g * Time.fixedDeltaTime;
        if (newVy < -maxFallSpeed)
        {
            newVy = -maxFallSpeed;
        }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, newVy);
    }

    public void CutJump()
    {
        if (rb.linearVelocityY > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocityY * secondJumpForceMultiplier);
        }
    }


}