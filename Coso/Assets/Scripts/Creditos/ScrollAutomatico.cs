using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Script realizado por deepseek v4.1 flash
/// <summary>
/// Aplica un scroll automático a la escena de los creditos
/// </summary>
public class ScrollAutomatico : MonoBehaviour
{
    [Header("Referencias")]
    public ScrollRect scrollRect;

    [Header("Ajustes de Scroll")]
    [Tooltip("Velocidad a la que suben los créditos (unidades por segundo).")]
    public float velocidadScroll = 0.05f;

    [Tooltip("Tiempo de espera en segundos antes de empezar a mover.")]
    public float tiempoEspera = 2f;

    void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();

        // Inicia la corrutina que mueve los créditos
        StartCoroutine(MoverCreditos());
    }

    IEnumerator MoverCreditos()
    {
        // 1. Posiciona el scroll al principio (tope)
        scrollRect.verticalNormalizedPosition = 1f;

        // 2. Espera el tiempo configurado
        yield return new WaitForSeconds(tiempoEspera);

        // 3. Bucle de movimiento mientras no llegue al final (0)
        while (scrollRect.verticalNormalizedPosition > 0.01f)
        {
            // Mueve la posición hacia 0 de forma lineal
            scrollRect.verticalNormalizedPosition -= velocidadScroll * Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        // 4. Asegura que se detenga exactamente en el fondo
        scrollRect.verticalNormalizedPosition = 0f;

        // 5. Cargamos la escena de mundo base
        GestionEscenas.GetInstance().CambiarEscena("MundoBase");
    }
}