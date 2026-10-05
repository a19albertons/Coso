using UnityEngine;

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

    public void FaceDirection(float direction)
    {
        if (direction == 0) return; // No hacemos nada si no hay movimiento
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Mathf.Sign(direction);
        transform.localScale = escala;
    }
    public void UpdateAnimations(Vector2 velocity, bool isGrounded)
    {
        animacion.SetFloat(VelocidadYHash, velocity.y);
        animacion.SetBool(IsWalkingHash, velocity.x >= 0.01f || velocity.x <= -0.01f);
        animacion.SetBool(EnSueloHash, isGrounded);
    }

    public void StopAnimacion()
    {
        animacion.SetFloat(VelocidadYHash, 0);
        animacion.SetBool(IsWalkingHash, false);
        animacion.SetBool(EnSueloHash, true);
    }
}