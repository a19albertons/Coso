using UnityEngine;

/// <summary>
/// Clase que gestiona las animaciones del jugador a partir de la información que le llega del PlayerController y PlayerMotor.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerVisuals : MonoBehaviour
{
    // Hardcodeo animaciones
    private static readonly int EnSueloHash = Animator.StringToHash("enSuelo");
    private static readonly int VelocidadYHash = Animator.StringToHash("velocidadY");
    private static readonly int IsWalkingHash = Animator.StringToHash("isMoving");

    // Componentes
    private Animator animacion;

    void Awake()
    {
        animacion = GetComponent<Animator>();

    }

    /// <summary>
    /// Hace que el jugador mire hacia la dirección del movimiento. Si no hay movimiento, no hace nada.
    /// </summary>
    /// <param name="direction"></param>
    public void FaceDirection(float direction)
    {
        if (direction == 0) return; // No hacemos nada si no hay movimiento
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Mathf.Sign(direction);
        transform.localScale = escala;
    }

    /// <summary>
    /// Actualiza las animaciones del jugador según la velocidad y si está en el suelo o no.
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="isGrounded"></param>
    public void UpdateAnimations(Vector2 velocity, bool isGrounded)
    {
        animacion.SetFloat(VelocidadYHash, velocity.y);
        animacion.SetBool(IsWalkingHash, velocity.x >= 0.01f || velocity.x <= -0.01f);
        animacion.SetBool(EnSueloHash, isGrounded);
    }

    /// <summary>
    /// Detiene las animaciones
    /// </summary>
    public void StopAnimacion()
    {
        animacion.SetFloat(VelocidadYHash, 0);
        animacion.SetBool(IsWalkingHash, false);
        animacion.SetBool(EnSueloHash, true);
    }
}