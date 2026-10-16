using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseDeathController : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds2 = new(2f);
    [SerializeField]
    protected GameObject deathEffect;

    protected new Collider2D collider;
    protected PlayerIsDeath playerIsDeath;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
        playerIsDeath = FindFirstObjectByType<PlayerIsDeath>();
        if (playerIsDeath == null)
        {
            Debug.LogError("No se encontró el componente PlayerIsDeath en la escena.");
        }
    }

    /// <summary>
    /// Detecta la colisión con el jugador e invoca al metodo encargado de gestionar la muerte del jugador.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerIsDeath.isDead)
        {
            if (GestionPowerUP.GetInstance().EstadoPowerUP() is not NoneState)
            {
                GestionPowerUP.GetInstance().PlayerPierdePowerUP();
                FindFirstObjectByType<PlayerInvencible>().ActivarInvencibilidad();
                return;
            }
            playerIsDeath.isDead = true;
            GestionVidas.GetInstance().PlayerPierdeVida();
            HandlePlayerDeath(other.gameObject);
        }
    }

    /// <summary>
    /// Obliga a implementar la gestión a toda clase que lo herede de como se hace la muerte. 
    /// </summary>
    /// <param name="player"></param>
    protected abstract void HandlePlayerDeath(GameObject player);


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