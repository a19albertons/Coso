using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona el menú de pausa del nivel, permitiendo al jugador pausar el juego, reanudarlo, reiniciar el nivel, salir al menú de niveles o salir del juego.
/// </summary>
public class PauseMenuLevel : MonoBehaviour
{
    [Header("Pause Menu UI")]
    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    private Button buttonResume;

    [SerializeField]
    private Button buttonRestart;

    [SerializeField]
    private Button buttonLevel;

    [SerializeField]
    private Button buttonGame;

    [Header("Player Input")]
    [SerializeField]
    private InputActionAsset playerInput;

    [Header("Player Controller Script")]
    [SerializeField]
    private PlayerController playerControllerScript;

    private InputAction pauseAction;
    private bool isPaused;

    void Awake()
    {
        pauseAction = playerInput.FindActionMap("Gameplay").FindAction("Pause");
        buttonResume.onClick.AddListener(Continuar);
        buttonRestart.onClick.AddListener(Restart);
        buttonLevel.onClick.AddListener(ExitLevel);
        buttonGame.onClick.AddListener(ExitGame);
    }

    void OnEnable()
    {
        pauseAction.Enable();
        pauseAction.performed += TogglePause;
    }

    void OnDisable()
    {
        pauseAction.performed -= TogglePause;
        pauseAction.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    /// <summary>
    /// Gestiona el boton de continuar, reanudando el juego y cerrando el menú de pausa.
    /// </summary>
    public void Continuar()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        playerControllerScript.enabled = true;
        Resume();
    }

    /// <summary>
    /// Gestiona el boton de reiniciar, reiniciando el nivel actual y reanudando el juego.
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;
        playerControllerScript.enabled = true;
        GestionPowerUP.GetInstance().PlayerPierdePowerUP();
        GestionVidas.GetInstance().PlayerPierdeVida();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }

    /// <summary>
    /// Gestiona el boton de salir al menú de niveles, cargando la escena del menú de niveles y reanudando el juego.
    /// </summary>
    public void ExitLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;
        playerControllerScript.enabled = true;
        GestionPowerUP.GetInstance().PlayerPierdePowerUP();
        GestionVidas.GetInstance().PlayerPierdeVida();
        SceneManager.LoadScene("MundoBase");
        Resume();
    }

    /// <summary>
    /// Gestiona el boton de salir del juego, cerrando la aplicación.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        playerControllerScript.enabled = false;
        EventSystem.current.SetSelectedGameObject(buttonResume.gameObject);
    }

    void Resume()
    {
        if (!isPaused)
        {
            return;
        }
        isPaused = false;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        playerControllerScript.enabled = true;
    }
}