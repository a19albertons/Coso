using UnityEngine;

/// <summary>
/// Clase que gestiona el cambio del jugador por un jugador muerto en la cinemática del final malo
/// </summary>
public class CambiarPlayerFinalMalo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instanciar jugador muerto")]
    private GameObject playerMuertoPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject playerMuerto = Instantiate(playerMuertoPrefab, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}