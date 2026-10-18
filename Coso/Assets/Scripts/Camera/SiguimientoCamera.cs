using UnityEngine;

/// <summary>
/// Clase que gestiona que la cámara siga al objeto que se le indique.
/// </summary>
public class SiguimientoCamera : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Objeto a seguir por la cámara")]
    private Transform target;

    [SerializeField]
    [Tooltip("Distancia de seguimiento de la cámara")]
    private Vector3 offset = new(0f, 5f, -10f);

    [SerializeField]
    [Tooltip("Velocidad de seguimiento de la cámara")]
    private float suavizado = 5f;

    /// <summary>
    /// Actualiza la posición de la cámara después de que todos los objetos hayan sido actualizados en el frame actual. Esto asegura que la cámara siga al jugador después de que este se haya movido.
    /// </summary>
    void LateUpdate()
    {
        Vector3 posicionDeseada = PosicionDeseada();

        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime), Quaternion.identity); // Mantener la rotación de la cámara en cero y optimiza la ejecución
    }

    /// <summary>
    /// Antiguo metodo que reubicaba la camara a donde esta el jugador sin suavizado. Actualmente descontinuado no respawneas en el nivel. 
    /// </summary>
    public void MuerteJugador()
    {
        transform.SetPositionAndRotation(PosicionDeseada(), Quaternion.identity);
    }

    /// <summary>
    /// Calculo los limites de la camara para que no se salga de los limites establecidos especialmente en el eje Y y Z.
    /// </summary>
    /// <returns></returns>
    private Vector3 PosicionDeseada()
    {
        Vector3 posicionDeseada = target.position + offset;
        posicionDeseada.z = -10f; // Asegurarse de que la cámara esté en la posición correcta en el eje Z
        if (posicionDeseada.y < 0f)
        {
            posicionDeseada.y = 0f; // Evitar que la cámara baje del suelo
        }
        if (posicionDeseada.y - posicionDeseada.z > 13f)
        {
            posicionDeseada.y = 3f; // Evitar que la cámara suba demasiado
        }
        return posicionDeseada;
    }
}