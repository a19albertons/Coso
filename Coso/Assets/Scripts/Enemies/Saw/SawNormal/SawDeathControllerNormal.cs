using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SawDeathControllerNormal : BaseDeathController
{
    /// <summary>
    /// Implementa la gestión de la muerte para la sierra normal
    /// </summary>
    /// <param name="player"></param>
    override protected void HandlePlayerDeath(GameObject player)
    {
        GameObject deathEffectInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        player.gameObject.SetActive(false);
        collider.isTrigger = false;
        StartCoroutine(MuertePersonaje.GuardarDatosJugador());
        StartCoroutine(MuertePersonaje.CargarEscenaAsync());
    }
}