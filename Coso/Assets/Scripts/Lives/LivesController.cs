using UnityEngine;

/// <summary>
/// Clase LivesController que gestiona la obtención de vidas extra por parte del jugador.
/// </summary>
public class LivesController : MonoBehaviour
{
    /// <summary>
    /// Detecta la colisión con el jugador y otorga una vida extra si el jugador la obtiene.
    /// </summary>
    /// <param name="other">Objeto que colision si no es player no se otorgará la vida extra.</param>
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            GeneralManager.GetInstance().SeObtieneVidaExtra();
            gameObject.SetActive(false); // Desactivar el objeto de vida extra al ser obtenida
        }
    }
}