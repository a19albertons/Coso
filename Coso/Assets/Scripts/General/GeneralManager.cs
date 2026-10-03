using UnityEngine;

/// <summary>
/// Clase GeneralManager que gestiona el estado general del juego, incluyendo la vida extra del jugador,
/// y las monedas acumuladas.
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
    /// Monedas acumuladas por el jugador.
    /// </summary>
    private int monedasAcumuladas = 0;

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
    /// Elimina la vida del jugador.
    /// </summary>
    public void PlayerRecibeGolpe()
    {
        if (recibir1Hit)
        {
            recibir1Hit = false;
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

    /// <summary>
    /// Suma una moneda al contador de monedas acumuladas. Si el jugador alcanza 100 monedas, se reinicia el contador y se puede proporcionar algún beneficio al jugador (por determinar).
    /// </summary>
    public void SumarMoneda()
    {
        monedasAcumuladas++;
        if (monedasAcumuladas >= 100)
        {
            monedasAcumuladas = 0;
            // Por determinar que se proporciona al jugador
        }
    }

    /// <summary>
    /// Devuelve el número de monedas acumuladas por el jugador. Función pensada para dar información en la UI
    /// </summary>
    /// <returns> El número de monedas acumuladas por el jugador. </returns>
    public int GetMonedasAcumuladas()
    {
        return monedasAcumuladas;
    }
}