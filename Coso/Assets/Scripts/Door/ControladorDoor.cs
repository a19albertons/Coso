using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControladorDoor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Nombre de la escena a cargar al entrar en la puerta")]
    private string sceneName;

    [SerializeField]
    [Tooltip("Estrellas requeridas para poder entrar al nivel")]
    private int requiredStars;

    [SerializeField]
    [Tooltip("Set de movimientos del jugador")]
    private InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputAction EntrarAlNivel;


    private AsyncOperation op;
    private bool isCalled = false;

    private bool enterLevel = false;

    void Awake()
    {
        playerActionMap = inputActions.FindActionMap("Gameplay");
        EntrarAlNivel = playerActionMap.FindAction("Puerta");
    }

    void OnEnable()
    {
        EntrarAlNivel.Enable();
    }

    void OnDisable()
    {
        EntrarAlNivel.Disable();
    }

    void Update()
    {
        if (EntrarAlNivel.IsPressed() && isCalled)
        {
            enterLevel = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isCalled)
            {
                op = SceneManager.LoadSceneAsync(sceneName);
                op.allowSceneActivation = false; // Evita que la escena se active automáticamente al terminar de cargar
                isCalled = true;
            }
        }
    }

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

        }
    }
}