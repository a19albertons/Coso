using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(PlayerVisuals))]
[RequireComponent(typeof(GroundDetector))]
public class ControllerCinematicPlayerInitial : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ubicacion final del jugador")]
    private Vector2 posicionFinal;

    private Vector2 posicionInicial;
    private Rigidbody2D rb;
    private PlayerVisuals animacion;
    private GroundDetector ground;
    private bool movimientoIniciado = false;

    void Awake()
    {
        posicionInicial = new Vector2(transform.position.x, transform.position.y);
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
        else
        {
            animacion.UpdateAnimations(Vector2.zero, ground.EnTierra);
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
        }
    }

    void FixedUpdate()
    {
        ground.ActualizarEstado();
        Moverse();
    }
}