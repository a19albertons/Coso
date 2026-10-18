using UnityEngine;

/// <summary>
/// Clase que gestiona que el fondo siga al objeto que se le indique.
/// </summary>
public class SiguimientoBackground : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Objeto a seguir por el fondo")]
    private Transform target;

    void Update() {
        transform.position = Vector2.Lerp(transform.position, new Vector2(target.position.x, transform.position.y), 2f); // Suavizado de la posición del fondo
    }
}
