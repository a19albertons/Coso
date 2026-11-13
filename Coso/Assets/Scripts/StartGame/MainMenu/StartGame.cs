using System;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona la pantalla de inicio del juego, permitiendo al jugador iniciar un nuevo juego o continuar desde un guardado previo.
/// </summary>
public class StartGame : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button exitGameButton;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject slots;

    /// <summary>
    /// Variable estatica que indica si estamos accediendo por la opción de juego nuevo o cargar juego
    /// </summary>
    [SerializeField]
    public static bool newGame = false;


    void Start()
    {
        newGameButton.onClick.AddListener(NewGame);

        continueButton.onClick.AddListener(ContinueGame);

        exitGameButton.onClick.AddListener(ExitGame);

        // Soporte para mando / teclado al empezar el juego, seleccionando el botón de nuevo juego por defecto
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
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
    }

    /// <summary>
    /// Metodo que actua sobre el boton de continuar
    /// </summary>
    private void ContinueGame()
    {
        newGame = false;

        mainMenu.SetActive(false);
        slots.SetActive(true);
    }

    /// <summary>
    /// Metodo que actua sobre el boton de cerrar el juego
    /// </summary>
    private void ExitGame()
    {
        Application.Quit();
    }
}