using UnityEngine;

/// <summary>
/// Clase PlayerAutomaticDeath que gestiona la muerte automática del jugador cuando cae por debajo de una altura específica en el eje Y.
/// </summary>
public class PlayerAutomaticDeath : MonoBehaviour
{
    [SerializeField]
    private float alturaMuerteEjeY = -6f;

    void Update()
    {
        if (transform.position.y < alturaMuerteEjeY)
        {
            GeneralManager.GetInstance().PlayerRecibeGolpe();
            FindFirstObjectByType<PlayerPositionSpawn>().SpawnPlayer();
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Restablecemos la velocidad del jugador a cero para evitar que siga cayendo
            FindFirstObjectByType<SiguimientoCamera>().MuerteJugador();
            
        }
    }
}