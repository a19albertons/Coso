using UnityEngine;
using UnityEngine.SceneManagement;

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
    private PowerUPStateMachine estadoPowerUP = new PowerUPStateMachine();



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
            estadoPowerUP.Initialize(new NoneState(FindFirstObjectByType<PlayerController>()));
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Cuando el singleton es destruido se desuscribe del evento de cambio de escena para evitar errores
    /// </summary>
    void OnDestroy ()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    /// <summary>
    /// Cuando se carga una nueva escena evalua el tipo de power up del jugador y restaura la referencia al player controller.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (estadoPowerUP.CurrentState)
        {
            case NoneState:
                estadoPowerUP.ChangeState(new NoneState(FindFirstObjectByType<PlayerController>()));
                break;
            case DoubleJumpState:
                estadoPowerUP.ChangeState(new DoubleJumpState(FindFirstObjectByType<PlayerController>()));
                break;
            default:
                estadoPowerUP.ChangeState(new NoneState(FindFirstObjectByType<PlayerController>()));
                break;
        }
    }

    /// <summary>
    /// Elimina el power up del jugador.
    /// </summary>
    public void PlayerPierdePowerUP()
    {
        if (estadoPowerUP != null)
        {
            estadoPowerUP.ChangeState(new NoneState(FindFirstObjectByType<PlayerController>()));
        }
    }

    /// <summary>
    /// Habilita el power up del jugador.
    /// </summary>
    public void PlayerObtienePowerUP(PowerUPStates powerUP)
    {
        estadoPowerUP.ChangeState(powerUP);
    }

    /// <summary>
    /// Indica si el jugador tiene el power up o no.
    /// </summary>
    /// <returns>Devuelve si el jugador tiene power up o no</returns>
    public PowerUPStates EstadoPowerUP()
    {
        return estadoPowerUP.CurrentState;
    }


}