using UnityEngine;

public class SawAnimation : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Velocidad de rotación de la sierra")]
    private float velocidadRotacion = 360f; // Grados por segundo

    void FixedUpdate()
    {
        // Rotar la sierra alrededor de su eje Z
        transform.Rotate(0f, 0f, velocidadRotacion * Time.fixedDeltaTime);
    }
}