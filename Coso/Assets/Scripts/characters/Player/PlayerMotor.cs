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
    public float JumpForce = 5f;

    [SerializeField]
    [Tooltip("Multiplicador de fuerza segundo salto")]
    public float SecondJumpForceMultiplier = 0.5f;

    [SerializeField]
    [Tooltip("Multiplicador de fuerza para cortar el salto")]
    public float CutJumpForceMultiplier = 0.5f;

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

    public int contadorSaltos = 0;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;       // gravedad manual
        rb.freezeRotation = true;
    }

    

    /// <summary>
    /// Mueve al jugador sobre el suelo aplicando la velocidad deseada en la dirección del suelo
    /// </summary>
    /// <param name="direction">dirrecion del personaje</param>
    /// <param name="groundNormal">Normal de la superficie sobre la que se mueve</param>
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

    /// <summary>
    /// Mueve al jugador en el aire aplicando un control limitado sobre su velocidad horizontal
    /// </summary>
    /// <param name="direction">Dirección de movimiento horizontal</param>
    public void MoveInAir(float direction)
    {
        float targetVelocityX = direction * moveSpeedWalking;
        float newVx = Mathf.MoveTowards(rb.linearVelocity.x, targetVelocityX, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newVx, rb.linearVelocity.y);
    }

    /// <summary>
    /// Aplica un salto al jugador estableciendo su velocidad vertical a jumpForce
    /// </summary>
    public void Jump()
    {
        GestionPowerUP.GetInstance().EstadoPowerUP().Jump();
    }

    /// <summary>
    /// Aplica la gravedad al jugador de forma manual
    /// </summary>
    /// <param name="JumpHeld"></param>
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

    /// <summary>
    /// Corta el salto del jugador si este se encuentra en el aire y su velocidad vertical es positiva, aplicando un multiplicador a la velocidad vertical
    /// </summary>
    public void CutJump()
    {
        if (rb.linearVelocityY > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocityY * CutJumpForceMultiplier);
        }
    }


}