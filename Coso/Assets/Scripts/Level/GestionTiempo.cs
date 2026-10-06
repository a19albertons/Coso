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

    void Awake()
    {
        tiempoRestante = tiempoAsignado;
    }

    void Update()
    {
        tiempoRestante -= Time.deltaTime;
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