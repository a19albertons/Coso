using UnityEngine;

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

    void LateUpdate() {
        Vector3 posicionDeseada = target.position + offset;
        posicionDeseada.z = -10f; // Asegurarse de que la cámara esté en la posición correcta en el eje Z
        if (posicionDeseada.y < 0f) {
            posicionDeseada.y = 0f; // Evitar que la cámara baje del suelo
        }
        if (posicionDeseada.y - posicionDeseada.z > 13f) {
            posicionDeseada.y = 3f; // Evitar que la cámara suba demasiado
        }
        
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime), Quaternion.identity); // Mantener la rotación de la cámara en cero y optimiza la ejecución
    }
}