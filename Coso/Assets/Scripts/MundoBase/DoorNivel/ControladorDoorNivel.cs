using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Clase ControladorDoor que gestiona la interacción del jugador con la puerta para entrar a un nivel.
/// </summary>
public class ControladorDoorNivel : MonoBehaviour
{
    [Header("Configuración de la puerta")]
    [Tooltip("Nombre de la escena a cargar al entrar en la puerta")]
    public string sceneName;

    [Tooltip("Estrellas requeridas para poder entrar al nivel")]
    public int requiredStars;

    [Header("Configuración de Input")]
    [SerializeField]
    [Tooltip("Set de movimientos del jugador")]
    private InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputAction EntrarAlNivel;


    private int cantidadEstrellas = 0;
    private bool requisitoEstrellas;
    private bool isEnter = false;
    private bool nivelPrevio = false;

    // Objetos hijos de la puerta
    [Header("Objetos hijos de la puerta")]
    [SerializeField]
    [Tooltip("Referencia a la valla que bloquea la puerta")]
    private GameObject vallaPuerta;
    [SerializeField]
    [Tooltip("Referencia al panel de información de la puerta")]
    private GameObject panelInfo;

    /// <summary>
    /// Se llama cuando el objeto se instancia. Inicializa las variables necesarias para el funcionamiento del controlador de la puerta.
    /// </summary>
    void Awake()
    {
        playerActionMap = inputActions.FindActionMap("Gameplay");
        EntrarAlNivel = playerActionMap.FindAction("Puerta");

        if (SaveManager.GetInstance().currentSave == null)
        {
            Debug.LogError("No se ha cargado ningún archivo de guardado. Asegúrate de que el SaveManager esté inicializado correctamente.");
            return;
        }
        if (sceneName.Equals("1-1") || sceneName.Equals("NivelPruebas"))
        {
            nivelPrevio = true;
        }
        SaveManager.GetInstance().currentSave.levels.ForEach(level =>
        {
            for (int i = 0; i < level.starsObtained.Length; i++)
            {
                if (level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Obtenida || level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Provisional)
                {
                    cantidadEstrellas++;
                }
            }
            ValidarNivelPrevio(level);
        }
        );

        // Valida la cantidad de estrellas obtenidas por el jugador
        if (cantidadEstrellas >= requiredStars)
        {
            requisitoEstrellas = true;
        }
        else
        {
            requisitoEstrellas = false;
        }

        // Valida si muestra la puerta bloqueada o desbloqueada según los requisitos de estrellas y el nivel previo
        if (requisitoEstrellas && nivelPrevio)
        {
            vallaPuerta.SetActive(false);
        }
        else
        {
            vallaPuerta.SetActive(true);
        }

        panelInfo.SetActive(false);
    }

    /// <summary>
    /// Valida si el nivel previo esta completo (se ha alcanzado el final)
    /// </summary>
    private void ValidarNivelPrevio(Level level)
    {
        // De lo indicado en la puerta actual
        String parte2 = sceneName.Split('-')[1];
        int parte2ConvertidaYCalculada = int.Parse(parte2) - 1;

        // Proveniente del bucle de niveles guardados
        String parte2NivelGuardado = level.identificadorNivel.Split('-')[1];
        int parte2NivelGuardadoConvertida = int.Parse(parte2NivelGuardado);

        if (parte2ConvertidaYCalculada.Equals(parte2NivelGuardadoConvertida) && level.isLevelCompleted != EnumStateLevelYStars.LevelYStars.NoObtenida)
        {
            nivelPrevio = true;
        }
    }

    /// <summary>
    /// Comprueba en cada fram si el jugador ha presionado el boton de entrar al nivel y si cumple los requisitos para entrar al nivel
    /// </summary>
    void Update()
    {
        if (EntrarAlNivel.IsPressed() && isEnter && requisitoEstrellas && nivelPrevio)
        {
            GestionEscenas.GetInstance().CambiarEscena(sceneName);
        }
    }

    /// <summary>
    /// Se llama cuando el jugador entra en el trigger de la puerta.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = true;
            panelInfo.SetActive(true);
        }
    }


    /// <summary>
    /// Se llama cuando el jugador sale del trigger de la puerta.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = false;
            panelInfo.SetActive(false);
        }
    }
}