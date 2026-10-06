using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase PlayerAutomaticDeath que gestiona la muerte automática del jugador cuando cae por debajo de una altura específica en el eje Y.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAutomaticDeath : MonoBehaviour
{
    [SerializeField]
    private float alturaMuerteEjeY = -6f;

    [SerializeField]
    private AudioClip sonidoMuerte;

    private bool isDead = false; // Variable para controlar si el jugador ya ha muerto y evitar múltiples llamadas a la muerte



    void Update()
    {
        ControlarMuerte();
    }

    private void ControlarMuerte()
    {
        if (transform.position.y < alturaMuerteEjeY && !isDead)
        {
            isDead = true; // Marca que el jugador ha muerto para evitar múltiples llamadas a la muerte
            // Empieza la carga de la escena de forma asincrona para evitar congelar el juego mientras se carga
            StartCoroutine(MuertePersonaje.CargarEscenaAsync());

            // Se hacen los cambios necesario en el jugador para simular la muerte
            gameObject.GetComponent<PlayerController>().enabled = false;
            GestionPowerUP.GetInstance().PlayerPierdePowerUP();
            GestionVidas.GetInstance().PlayerPierdeVida();
            AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position);

            // Se actualizan los datos del jugador como numero de vidas, perdida de powerup, monedas...
            StartCoroutine(MuertePersonaje.GuardarDatosJugador());
        }
    }


}