using UnityEngine;

/// <summary>
/// Clase GestionTiempo que gestiona el tiempo del nivel.
/// </summary>
public class GestionTiempo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Tiempo asignado al nivel en segundos")]
    private float tiempoAsignado;

    private float tiempoRestante;

    // Variable para controlar si la ejecución de la muerte del personaje ya se ha realizado
    private bool isExecuted = false;

    void Awake()
    {
        tiempoRestante = tiempoAsignado;
    }

    void Update()
    {
        ActualizarTiempoRestante();
    }

    /// <summary>
    /// Actualiza el tiempo restante del nivel y maneja la muerte del personaje si el tiempo se agota.
    /// </summary>
    private void ActualizarTiempoRestante()
    {
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            if (!isExecuted)
            {
                GestionPowerUP.GetInstance().PlayerPierdePowerUP();
                GestionVidas.GetInstance().PlayerPierdeVida();
                StartCoroutine(MuertePersonaje.GuardarDatosJugador());
                StartCoroutine(MuertePersonaje.CargarEscenaAsync());
                isExecuted = true;
            }
        }
    }

    /// <summary>
    /// Obtiene el tiempo restante del nivel.
    /// </summary>
    /// <returns>Devuelve el tiempo restante en segundos.</returns>
    public string TiempoRestante()
    {
        return tiempoRestante.ToString("F0");
    }
}