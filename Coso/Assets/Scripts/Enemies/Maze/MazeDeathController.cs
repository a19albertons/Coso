using UnityEngine;

/// <summary>
/// Clase que controla la muerte especifica del jugador por causa de maze
/// </summary>
public class MazeDeathController : BaseDeathController
{

    /// <summary>
    /// Implementa la gestión de la muerte para el jugador al colisionar con maze
    /// </summary>
    /// <param name="player"></param>
    override protected void HandlePlayerDeath(GameObject player)
    {
        Vector3 calculoPosicionInstancia = new Vector3(transform.position.x, transform.position.y - collider.bounds.extents.y, transform.position.z);
        GameObject deathEffectInstance = Instantiate(deathEffect, calculoPosicionInstancia, Quaternion.identity);
        player.gameObject.SetActive(false);
        collider.isTrigger = false;
        StartCoroutine(MuertePersonaje.GuardarDatosJugador());
        StartCoroutine(MuertePersonaje.CargarEscenaAsync());
    }
}