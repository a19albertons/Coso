using System.Collections;
using UnityEngine;

/// <summary>
/// Clase PlayerInvencible que gestiona la invencibilidad del jugador durante un tiempo determinado, alternando la visibilidad del jugador para crear un efecto de parpadeo.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerInvencible : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Capa original del jugador. El indice de la capa original del jugador.")]
    private int playerLayerOriginal;
    [SerializeField]
    [Tooltip("Capa del jugador cuando es invencible. El indice de la capa del jugador cuando es invencible.")]
    private int playerLayerInvencible;

    [SerializeField]
    [Tooltip("Duración de la invencibilidad en segundos. Puedes morir por otras causas, pero no por enemigos.")]
    private float invencibleDuration = 2f;
    [SerializeField]
    [Tooltip("Intervalo de parpadeo en segundos durante la invencibilidad.")]
    private float blinkInterval = 0.2f;


    /// <summary>
    /// Metodo que invoca la corutina que activa la invencibilidad del jugador.
    /// </summary>
    public void ActivarInvencibilidad()
    {
        StartCoroutine(InvencibilidadCoroutine());
    }

    /// <summary>
    /// Corutina que activa la invencibilidad del jugador durante un tiempo determinado, alternando la visibilidad del jugador para crear un efecto de parpadeo.
    /// </summary>
    /// <returns></returns>
    IEnumerator InvencibilidadCoroutine()
    {
        gameObject.layer = playerLayerInvencible;


        float elapsedTime = 0f;
        while (elapsedTime < invencibleDuration)
        {
            // Alternar la visibilidad del jugador para crear un efecto de parpadeo
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;

            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }
        gameObject.layer = playerLayerOriginal;
        GetComponent<SpriteRenderer>().enabled = true;
    }

}