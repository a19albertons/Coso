using UnityEngine;

/// <summary>
/// Clase GestionVidas que gestiona la cantidad de vidas del jugador
/// </summary>
public class GestionVidas : MonoBehaviour
{
    /// <summary>
    /// Instancia única de GestionVidas. Se asegura que solo haya una instancia en la escena y que no se destruya al cambiar de escena.
    /// </summary>
    static GestionVidas instance;

    /// <summary>
    /// Devuelve la instancia única de GestionVidas. Si no existe, devuelve null.
    /// </summary>
    /// <returns>La instancia única de GestionVidas.</returns>
    public static GestionVidas GetInstance()
    {
        return instance;
    }

    [SerializeField]
    [Tooltip("Cantidad de vidas del jugador a restaurar y al iniciar el juego")]
    private int vidas = 5; // Cantidad de vidas del jugador

    private int vidasActuales; // Cantidad de vidas actuales del jugador

    /// <summary>
    /// Asegura que solo haya una instancia de GestionVidas en la escena y que no se destruya al cambiar de escena.
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
                vidasActuales = SaveManager.GetInstance().currentSave.playerInfo.vidasActuales;
            }
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Procesa la perdida de una vida
    /// </summary>
    public void PlayerPierdeVida()
    {
        if (vidasActuales > 0)
        {
            vidasActuales--;
        }
    }

    /// <summary>
    /// Procesa la obtención de una vida extra
    /// </summary>
    public void PlayerObtieneVida()
    {
        vidasActuales++;
    }

    /// <summary>
    /// Restablece el valor de vidas actuales al valor inicial de vidas. Función pensada para cuando quedas sin vidas o empiezas el juego por primera vez
    /// </summary>
    public void ResetVidas()
    {
        vidasActuales = vidas;
    }

    /// <summary>
    /// Devuelve la cantidad de vidas actuales del jugador. Pensado para ser utilizado en la GUI
    /// </summary>
    /// <returns> La cantidad de vidas actuales del jugador. </returns>
    public string GetVidasActuales()
    {
        return vidasActuales.ToString();
    }

    /// <summary>
    /// Devuelve la cantidad de vidas actuales del jugador como un entero. Pensado para ser utilizado en la logica del juego
    /// </summary>
    /// <returns></returns>
    public int GetVidasActualesInt()
    {
        return vidasActuales;
    }
}