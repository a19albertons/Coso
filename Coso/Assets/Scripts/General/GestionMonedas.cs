using UnityEngine;

/// <summary>
/// Clase que gestiona la cantidad de monedas acumuladas por el jugador a lo largo del juego en sus distintas escenas
/// </summary>
public class GestionMonedas : MonoBehaviour
{
    /// <summary>
    /// Monedas acumuladas por el jugador.
    /// </summary>
    private int monedasAcumuladas = 0;

    /// <summary>
    /// Instancia única de GestionMonedas. Se asegura que solo haya una instancia en la escena y que no se destruya al cambiar de escena.
    /// </summary>
    static GestionMonedas instance;

    /// <summary>
    /// Devuelve la instancia única de GestionMonedas. Si no existe, devuelve null.
    /// </summary>
    /// <returns> La instancia única de GestionMonedas. </returns>
    public static GestionMonedas GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Asegura que solo haya una instancia de GestionMonedas en la escena y que no se destruya al cambiar de escena.
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (SaveManager.GetInstance().currentSave == null)
            {
                Debug.LogError("No se ha cargado un archivo de guardado. Se recomienda iniciar el juego desde la escena de inicio para cargar correctamente los datos del jugador.");
            }
            else
            {
                monedasAcumuladas = SaveManager.GetInstance().currentSave.playerInfo.monedasActuales;
            }
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
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

        }
    }

    /// <summary>
    /// Devuelve el número de monedas acumuladas por el jugador. Función pensada para dar información en la UI
    /// </summary>
    /// <returns> El número de monedas acumuladas por el jugador. </returns>
    public string GetMonedasAcumuladas()
    {
        return monedasAcumuladas.ToString();
    }

    /// <summary>
    /// Devuelve la cantidad de monedas acumuladas del jugador como un entero. Pensado para ser utilizado en la logica del juego
    /// </summary>
    /// <returns></returns>
    public int GetMonedasAcumuladasInt()
    {
        return monedasAcumuladas;
    }
}