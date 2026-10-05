using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SawDeathController : BaseDeathController
{
    override protected void HandlePlayerDeath(GameObject player)
    {
        GameObject deathEffectInstance = Instantiate(deathEffect, player.transform.position, Quaternion.identity);
        player.gameObject.SetActive(false);
        collider.isTrigger = false;
        StartCoroutine(ReiniciarNivel(player.gameObject, deathEffectInstance));
    }
}