using UnityEngine;

/// <summary>
/// Clase que gestiona que ambos jugadores salgan de la camara, la camara y el fondo no se muevan y las animaciones de los personajes.
/// </summary>
public class ControllerEndScene : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Punto donde ambos personaje no se vean en la camara")]
    private Vector2 puntoFueraDeCamara;

    [Header("Camara")]
    [SerializeField]
    [Tooltip("Objeto camara que contiene el script de seguimiento de camara")]
    private new SiguimientoCamera camera;

    [Header("Fondo")]
    [SerializeField]
    [Tooltip("Objeto del fondo que sigue al jugador")]
    private SiguimientoBackground fondo;

    [SerializeField]
    [Tooltip("Efecto parallax del fondo")]
    private ParallaxBackground efectoParallax;

    [Header("Personajes")]
    [SerializeField]
    [Tooltip("Objeto del jugador")]
    private GameObject jugador;

    [SerializeField]
    [Tooltip("Objeto de la novia")]
    private GameObject novia;

    private bool moverse = false;


    void Awake()
    {
        camera.enabled = false; // Evita que al mover el jugador le siga la camara
        fondo.enabled = false; // Evita que al mover el jugador le siga el fondo
        efectoParallax.enabled = false; // Deshabilita el efecto parallax del fondo para que no se mueva al mover el jugador
    }

    public void IniciarMovimiento()
    {
        moverse = true;
    }

    void FixedUpdate()
    {
        MoverPersonajes();
    }
    private void MoverPersonajes()
    {
        if (!moverse)
        {
            return;
        }
        novia.GetComponent<SpriteRenderer>().flipX = false;
        jugador.transform.position = Vector2.MoveTowards(jugador.transform.position, puntoFueraDeCamara, 3f * Time.fixedDeltaTime);
        novia.transform.position = Vector2.MoveTowards(novia.transform.position, puntoFueraDeCamara, 3f * Time.fixedDeltaTime);
    }

    void Update()
    {
        Animaciones();
    }
    private void Animaciones()
    {
        if (moverse)
        {
            jugador.GetComponent<ControllerCinematicPlayerInitial>().enabled = false; // Deshabilita el script para que no bloquee la gestión de animaciones del actual
            jugador.GetComponent<PlayerVisuals>().UpdateAnimations(Vector2.right, true);
        }
        else
        {
            jugador.GetComponent<PlayerVisuals>().UpdateAnimations(Vector2.zero, true);
        }
    }

}