using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona la pantalla de inicio del juego, permitiendo al jugador iniciar un nuevo juego o continuar desde un guardado previo.
/// </summary>
public class StartGame : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button loadGameButton;

    [SerializeField]
    private Button settingsButton;

    [SerializeField]
    private Button exitGameButton;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject slots;

    [SerializeField]
    private GameObject settings;

    private GameObject ultimoSelecionado;

    /// <summary>
    /// Variable estatica que indica si estamos accediendo por la opción de juego nuevo o cargar juego
    /// </summary>
    public static bool newGame = false;


    void Start()
    {
        newGameButton.onClick.AddListener(NewGame);

        loadGameButton.onClick.AddListener(LoadGame);

        settingsButton.onClick.AddListener(Settings);

        exitGameButton.onClick.AddListener(ExitGame);

        // Definimos el boton inicial y lo forzamos la primera vez
        ultimoSelecionado = newGameButton.gameObject;
        EventSystem.current.SetSelectedGameObject(ultimoSelecionado);
    }

    void OnEnable()
    {
        // Soporte para mando / teclado al empezar el juego, seleccionando el botón de nuevo juego por defecto
        EventSystem.current.SetSelectedGameObject(ultimoSelecionado);
    }

    /// <summary>
    /// Metodo que actua sobre el boton nuevo juego
    /// </summary>
    private void NewGame()
    {
        // Permite que todos los slots sean clickables
        newGame = true;

        mainMenu.SetActive(false);
        slots.SetActive(true);

        // actualizamos el boton
        ultimoSelecionado = newGameButton.gameObject;
    }

    /// <summary>
    /// Metodo que actua sobre el boton de continuar
    /// </summary>
    private void LoadGame()
    {
        newGame = false;

        mainMenu.SetActive(false);
        slots.SetActive(true);

        // actualizamos el boton
        ultimoSelecionado = loadGameButton.gameObject;
    }

    /// <summary>
    /// Metodo que muestra las opciones de configuración y oculta el menu principal
    /// </summary>
    private void Settings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);

        // actualizamos el boton
        ultimoSelecionado = settingsButton.gameObject;
    }

    /// <summary>
    /// Metodo que actua sobre el boton de cerrar el juego
    /// </summary>
    private void ExitGame()
    {
        Application.Quit();
    }
}