using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase ControladorDoor que gestiona la interacción del jugador con la puerta para entrar a un nivel.
/// </summary>
public class ControladorDoor : MonoBehaviour
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


    private AsyncOperation op;
    private bool isCalled = false;
    private bool enterLevel = false;
    private int cantidadEstrellas = 0;
    private bool cumpleRequisitos;
    private bool isEnter = false;

    // Objetos hijos de la puerta
    private GameObject vallaPuerta;
    private GameObject panelInfo;

    /// <summary>
    /// Se llama cuando el objeto se instancia. Inicializa las variables necesarias para el funcionamiento del controlador de la puerta.
    /// </summary>
    void Awake()
    {
        playerActionMap = inputActions.FindActionMap("Gameplay");
        EntrarAlNivel = playerActionMap.FindAction("Puerta");
        vallaPuerta = GameObject.Find("Bloqueada");
        panelInfo = GameObject.Find("Info");

        if (SaveManager.GetIntance().currentSave == null)
        {
            Debug.LogError("No se ha cargado ningún archivo de guardado. Asegúrate de que el SaveManager esté inicializado correctamente.");
            return;
        }
        SaveManager.GetIntance().currentSave.levels.ForEach(level =>
        {
            for (int i = 0; i < level.starsObtained.Length; i++)
            {
                if (level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Obtenida || level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Provisional)
                {
                    cantidadEstrellas++;
                }
            }
        }
        );

        if (cantidadEstrellas >= requiredStars)
        {
            vallaPuerta.SetActive(false);
            cumpleRequisitos = true;
        }
        else
        {
            vallaPuerta.SetActive(true);
            cumpleRequisitos = false;
        }

        panelInfo.SetActive(false);
    }

    /// <summary>
    /// Se llama cuando el objeto se activa. Habilita la acción de entrar al nivel para que pueda ser detectada mientras el objeto está activo.
    /// </summary>
    void OnEnable()
    {
        EntrarAlNivel.Enable();
    }

    /// <summary>
    /// Se llama cuando el objeto se desactiva. Deshabilita la acción de entrar al nivel para evitar que se siga ejecutando mientras el objeto está desactivado.
    /// </summary>
    void OnDisable()
    {
        EntrarAlNivel.Disable();
    }

    /// <summary>
    /// Comprueba en cada fram si el jugador ha presionado el boton de entrar al nivel y si cumple los requisitos para entrar al nivel
    /// </summary>
    void Update()
    {
        if (EntrarAlNivel.IsPressed() && isEnter && cumpleRequisitos)
        {
            enterLevel = true;
        }
    }

    /// <summary>
    /// Se llama cuando el jugador entra en el trigger de la puerta. Inicia la carga asíncrona de la escena si no se ha llamado previamente.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isCalled)
            {
                StartCoroutine(CargarEscenaAsync());
                isCalled = true;
            }
            isEnter = true;
            panelInfo.SetActive(true);
        }
    }

    /// <summary>
    /// Se llama cuando el jugador permanece dentro del trigger de la puerta y el boton de abrir la puesta ha sido presionado
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (enterLevel)
            {
                other.gameObject.GetComponent<PlayerController>().enabled = false;
                op.allowSceneActivation = true; // Permite que la escena se active automáticamente al terminar de cargar
            }

        }
    }


    /// <summary>
    /// Se llama cuando el jugador sale del trigger de la puerta. Si la escena está cargada, se descarga de forma asíncrona..
    /// Problema conocido : la escena nunca se descarga se queda siempre en isloading una vez has entrado
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
                isCalled = false;
            }
            isEnter = false;
            panelInfo.SetActive(false);
        }
    }

    /// <summary>
    /// Carga la escena en un hilo secundario evitando secuestrar el hilo principal
    /// </summary>
    /// <returns></returns>
    private IEnumerator CargarEscenaAsync()
    {
        op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false; // Evita que la escena se active automáticamente al terminar de cargar
        while (!op.isDone)
        {
            yield return null;
        }
    }
}