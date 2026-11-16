using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla la pantalla de "Has perdido" que solo puede saltar cuanto tu recuento de vidas es 0 o inferior
/// </summary>
public class ControllerHasPerdido : MonoBehaviour
{
    [SerializeField]
    private GameObject TextoContinuar;
    private bool isSaved = false; // Variable para controlar si los datos del jugador ya han sido guardados
    private InputAction continuarBoton; // Acción de entrada para continuar

    [SerializeField]
    private InputActionAsset accionesJugador;


    void Awake()
    {
        InputActionMap map = accionesJugador.FindActionMap("Gameplay");
        continuarBoton = map.FindAction("Jump");
    }

    void OnEnable()
    {
        continuarBoton.Enable();
    }

    void OnDisable()
    {
        continuarBoton.Disable();
    }

    void Start()
    {
        RestaurarProgreso();
        RestaurarDatosJugador();

    }

    void Update()
    {
        MostrarBotonContinuar();
        VolverMundoBase();
    }


    /// <summary>
    /// Restaura los datos del jugador como el powerup y el numero de vidas actuales al valor inicial definido
    /// </summary>
    private void RestaurarDatosJugador()
    {
        if (SaveManager.GetInstance().currentSave == null)
        {
            Debug.LogError("No se encontró un archivo de guardado actual.");
            return;
        }
        GestionPowerUP.GetInstance().PlayerPierdePowerUP(); // Llama al método PlayerPierdePowerUP() para restablecer el power-up del jugador
        GestionVidas.GetInstance().ResetVidas();
        SaveManager.GetInstance().currentSave.playerInfo.powerUpActual = PowerUPType.None; // Restablece el power-up actual a None
        SaveManager.GetInstance().currentSave.playerInfo.vidasActuales = GestionVidas.GetInstance().GetVidasActualesInt(); // Restablece las vidas actuales a su valor inicial

        //Operaciones de guardado
        SaveManager.GetInstance().SaveGame(1); // Guarda el juego en el archivo de guardado 1
        isSaved = true; // Marca que los datos del jugador han sido guardados
    }

    /// <summary>
    /// Muestra el botón de continuar en la pantalla para que el jugador pueda presionarlo y continuar con el juego.
    /// </summary>
    private void MostrarBotonContinuar()
    {
        if (isSaved)
        {
            TextoContinuar.SetActive(true);
        }
    }


    /// <summary>
    /// Detecta si el jugador ha presionado la tecla de continuar y actualiza la variable "continuar" en consecuencia. Para iniciar la escena configurada
    /// </summary>
    private void VolverMundoBase()
    {
        if (continuarBoton.IsPressed() && isSaved)
        {
            GestionEscenas.GetInstance().CambiarEscena("MundoBase");
        }
    }

    /// <summary>
    /// Restaurar el progreso del jugador eliminando todos los avances provisionales en el archivo de guardado actual.
    /// </summary>
    private void RestaurarProgreso()
    {
        if (SaveManager.GetInstance().currentSave == null)
        {
            Debug.LogError("No se encontró un archivo de guardado actual.");
            return;
        }
        SaveManager.GetInstance().currentSave.levels.ForEach(level =>
        {
            if (level.isLevelCompleted == EnumStateLevelYStars.LevelYStars.Provisional)
            {
                level.isLevelCompleted = EnumStateLevelYStars.LevelYStars.NoObtenida;
            }
            for (int i = 0; i < level.starsObtained.Length; i++)
            {
                if (level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Provisional)
                {
                    level.starsObtained[i] = EnumStateLevelYStars.LevelYStars.NoObtenida;
                }
            }
        });
    }
}