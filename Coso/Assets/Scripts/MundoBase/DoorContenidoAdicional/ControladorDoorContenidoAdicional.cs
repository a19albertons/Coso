using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Clase ControladorDoor que gestiona la interacción del jugador para el contenido adicional
/// </summary>
public class ControladorDoorContenidoAdicional : MonoBehaviour
{
    [Header("Configuración de la puerta")]
    [Tooltip("Nombre de la escena a cargar al entrar en la puerta")]
    public string sceneName;

    [Header("Configuración de Input")]
    [SerializeField]
    [Tooltip("Set de movimientos del jugador")]
    private InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputAction EntrarAlNivel;

    private bool isEnter = false;

    // Objetos hijos de la puerta
    [Header("Objetos hijos de la puerta")]
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
        panelInfo.SetActive(false);
    }

    /// <summary>
    /// Comprueba en cada fram si el jugador ha presionado el boton de entrar al nivel y si cumple los requisitos para entrar al nivel
    /// </summary>
    void Update()
    {
        if (EntrarAlNivel.IsPressed() && isEnter)
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