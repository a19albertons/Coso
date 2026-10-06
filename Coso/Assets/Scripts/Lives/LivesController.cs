using UnityEngine;

/// <summary>
/// Clase DoubleJumpController que gestiona la obtención de doble salto por parte del jugador.
/// </summary>
public class DoubleJumpController : MonoBehaviour
{
    /// <summary>
    /// Detecta la colisión con el jugador y otorga una vida extra si el jugador la obtiene.
    /// </summary>
    /// <param name="other">Objeto que colision si no es player no se otorgará la vida extra.</param>
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            GestionPowerUP.GetInstance().PlayerObtienePowerUP(new DoubleJumpState(other.GetComponent<PlayerController>()));
            gameObject.SetActive(false); // Desactivar el objeto de vida extra al ser obtenida
        }
    }
}