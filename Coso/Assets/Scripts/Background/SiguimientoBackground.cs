using UnityEngine;

public class SiguimientoBackground : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Objeto a seguir por el fondo")]
    private Transform target;

    [SerializeField]
    [Tooltip("Factor de variación eje Y sobre personaje")]
    private float factorY = 0.9f;    



    void Update() {
        transform.position = Vector2.Lerp(transform.position, new Vector2(target.position.x, target.position.y * factorY ), 2f); // Suavizado de la posición del fondo
    }
}
