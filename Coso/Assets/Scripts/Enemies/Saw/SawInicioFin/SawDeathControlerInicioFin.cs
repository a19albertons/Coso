using UnityEngine;

/// <summary>
/// Clase que controla la gestión de muerte con las sierras de tipo inicio-fin.
/// </summary>
public class SawDeathControlerInicioFin : BaseDeathController
{
    /// <summary>
    /// Personaliza la detección de colisiones con el jugador para gestionar la muerte especifica de este tipo de caso
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        HandlePlayerDeath(other.gameObject);
    }

    /// <summary>
    /// Implementa la gestión de la muerte del jugador para este tipo de sierra.
    /// </summary>
    /// <param name="player"></param>
    protected override void HandlePlayerDeath(GameObject player)
    {
        GestionPowerUP.GetInstance().PlayerPierdePowerUP();
        GestionVidas.GetInstance().PlayerPierdeVida();
        GameObject deathEffectInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        player.gameObject.SetActive(false);
        collider.isTrigger = false;
        StartCoroutine(MuertePersonaje.GuardarDatosJugador());
        StartCoroutine(MuertePersonaje.CargarEscenaAsync());
    }
}