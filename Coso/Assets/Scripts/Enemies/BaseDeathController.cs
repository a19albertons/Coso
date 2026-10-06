using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BaseDeathController : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds2 = new(2f);
    [SerializeField]
    protected GameObject deathEffect;

    protected new Collider2D collider;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Detecta la colisión con el jugador e invoca al metodo encargado de gestionar la muerte del jugador.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GestionPowerUP.GetInstance().EstadoPowerUP())
            {
                GestionPowerUP.GetInstance().PlayerPierdePowerUP();
                FindFirstObjectByType<PlayerInvencible>().ActivarInvencibilidad();
                return;
            }
            HandlePlayerDeath(other.gameObject);
        }
    }

    /// <summary>
    /// Maneja la muerte del jugador. Debe ser implementado por las clases derivadas para definir el comportamiento específico de la muerte del jugador en cada tipo de enemigo.
    /// </summary>
    /// <param name="player"></param>
    protected virtual void HandlePlayerDeath(GameObject player)
    {
    }

    /// <summary>
    /// Corutina que encapsula el patron comun de reinicio del nivel tras la muerte del jugador. Se encarga de esperar un tiempo, reubicar al jugador, restablecer la cámara y destruir el efecto de muerte.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="deathEffectInstance"></param>
    /// <returns></returns>
    protected IEnumerator ReiniciarNivel(GameObject player, GameObject deathEffectInstance)
    {
        yield return _waitForSeconds2;
        // Posición spwawn del jugador
        player.GetComponent<PlayerPositionSpawn>().SpawnPlayer();
        // Llamar al método MuerteJugador() de SiguimientoCamera para restablecer la posición de la cámara
        FindFirstObjectByType<SiguimientoCamera>().MuerteJugador();
        // Restablecemos el collider para que pueda volver a detectar colisiones
        collider.isTrigger = true;
        // Destruir el efecto de muerte después de un tiempo
        Destroy(deathEffectInstance);

    }

}