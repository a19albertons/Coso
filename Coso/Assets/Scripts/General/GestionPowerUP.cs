using UnityEngine;

/// <summary>
/// Clase GestionPowerUP que gestiona el estado general del juego, incluyendo la vida extra del jugador,
/// y las monedas acumuladas.
/// </summary>
public class GestionPowerUP : MonoBehaviour
{
    /// <summary>
    /// Instancia única de GestionPowerUP. Se asegura que solo haya una instancia en la escena y que no se destruya al cambiar de escena.
    /// </summary>
    static GestionPowerUP instance;
    /// <summary>
    /// Variable que controla el estado de la vida extra del jugador.
    /// </summary>
    private bool hayPowerUP = false;



    /// <summary>
    /// Devuelve la instancia única de GeneralManager. Si no existe, devuelve null.
    /// </summary>
    /// <returns>La unica instancia de GeneralManager.</returns>
    public static GestionPowerUP GetInstance()
    {
        return instance;
    }


    /// <summary>
    /// Asegura que solo haya una instancia de GestionPowerUP en la escena y que no se destruya al cambiar de escena.
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
    /// Elimina el power up del jugador.
    /// </summary>
    public void PlayerPierdePowerUP()
    {
        if (hayPowerUP)
        {
            hayPowerUP = false;
        }
    }

    /// <summary>
    /// Habilita el power up del jugador.
    /// </summary>
    public void PlayerObtienePowerUP()
    {
        hayPowerUP = true;
    }

    /// <summary>
    /// Indica si el jugador tiene el power up o no.
    /// </summary>
    /// <returns>Devuelve si el jugador tiene power up o no</returns>
    public bool EstadoPowerUP()
    {
        return hayPowerUP;
    }


}