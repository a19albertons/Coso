using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SawDeathController : MonoBehaviour
{
    [SerializeField]
    private GameObject deathEffect;

    private new Collider2D collider;


    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject deathEffectInstance = Instantiate(deathEffect, other.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
            collider.isTrigger = false;
            StartCoroutine(ReiniciarNivel(other.gameObject, deathEffectInstance));
        }
    }

    IEnumerator ReiniciarNivel(GameObject player, GameObject deathEffectInstance)
    {
        yield return new WaitForSeconds(2f);
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