using UnityEngine;

public class MazeDeathController : BaseDeathController
{

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