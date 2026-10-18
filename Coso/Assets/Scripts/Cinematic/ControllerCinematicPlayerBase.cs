using UnityEngine;

/// <summary>
/// Clase base que gestiona los movimiento del jugador en las distintas cinemáticas de forma estandarizada como si fuese una plantilla.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerVisuals))]
[RequireComponent(typeof(GroundDetector))]
public abstract class ControllerCinematicPlayerBase : MonoBehaviour
{
    [Tooltip("Ubicacion final del jugador")]
    protected abstract Vector2 posicionFinal { get; }


    private Rigidbody2D rb;
    private PlayerVisuals animacion;
    private GroundDetector ground;
    private bool movimientoIniciado = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animacion = GetComponent<PlayerVisuals>();
        ground = GetComponent<GroundDetector>();
    }

    public void IniciarMovimiento()
    {
        movimientoIniciado = true;
    }

    void Update()
    {
        if (movimientoIniciado)
        {
            animacion.UpdateAnimations(Vector2.right, ground.EnTierra);
        }
    }


    private void Moverse()
    {
        if (!movimientoIniciado)
        {
            return;
        }
        float newX = Mathf.MoveTowards(transform.position.x, posicionFinal.x, 3f * Time.fixedDeltaTime);
        float newY = Mathf.MoveTowards(transform.position.y, posicionFinal.y, 3f * Time.fixedDeltaTime);
        rb.MovePosition(new Vector2(newX, newY));
        if (Vector2.Distance(transform.position, posicionFinal) < 0.1f)
        {
            movimientoIniciado = false;
            animacion.UpdateAnimations(Vector2.zero, ground.EnTierra);
        }
    }

    void FixedUpdate()
    {
        ground.ActualizarEstado();
        Moverse();
    }
}