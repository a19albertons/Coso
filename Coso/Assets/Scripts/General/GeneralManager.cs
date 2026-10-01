using UnityEngine;

/// <summary>
/// Clase GeneralManager que gestiona el estado general del juego, incluyendo la vida extra del jugador.
/// </summary>
public class GeneralManager : MonoBehaviour
{
    /// <summary>
    /// Instancia única de GeneralManager. Se asegura que solo haya una instancia en la escena y que no se destruya al cambiar de escena.
    /// </summary>
    static GeneralManager instance;
    /// <summary>
    /// Variable que controla el estado de la vida extra del jugador.
    /// </summary>
    private bool recibir1Hit = false;

    /// <summary>
    /// Devuelve la instancia única de GeneralManager. Si no existe, devuelve null.
    /// </summary>
    /// <returns>La unica instancia de GeneralManager.</returns>
    public static GeneralManager GetInstance()
    {
        return instance;
    }


    /// <summary>
    /// Asegura que solo haya una instancia de GeneralManager en la escena y que no se destruya al cambiar de escena.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Comprueba si el jugador tiene una vida extra disponible. Si la tiene, se consume y no se destruye el jugador. Si no la tiene, se destruye el jugador.
    /// </summary>
    /// <param name="player">El jugador que recibe el golpe.</param>
    public void PlayerRecibeGolpe(GameObject player)
    {
        if (recibir1Hit)
        {
            recibir1Hit = false;
        }
        else
        {
            Destroy(player);
        }
    }

    /// <summary>
    /// Establece que el jugador tiene una vida extra disponible.
    /// </summary>
    public void SeObtieneVidaExtra()
    {
        recibir1Hit = true;
    }

    /// <summary>
    /// Indica si el jugador tiene una vida extra disponible. Función pensada para dar información en la UI
    /// </summary>
    /// <returns>true si el jugador tiene una vida extra disponible, false en caso contrario </returns>
    public bool EstadoVidaExtra()
    {
        return recibir1Hit;
    }
}