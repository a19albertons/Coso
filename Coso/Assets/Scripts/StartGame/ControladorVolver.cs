using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// ControladorVolver controla que puedas volver al menu principal desde la carga del nuevo juego/cargar partida
/// </summary>
public class ControladorVolver : MonoBehaviour
{
    [Header("boton para raton y moviles")]
    [SerializeField]
    private Button botonVolver;

    [Header("Objetos a configurar")]

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject otro;

    [Header("Controles para mando y teclado")]
    [SerializeField]
    private InputActionAsset inputAction;
    private InputActionMap gameplayActionMap;

    private InputAction retrocederMainMenu;
    


    void Awake()
    {
        botonVolver.onClick.AddListener(VolverAtras);
        gameplayActionMap = inputAction.FindActionMap("Gameplay");
        retrocederMainMenu = gameplayActionMap.FindAction("RetrocederMainMenu");
    }

    void OnEnable()
    {
        retrocederMainMenu.Enable();
    }

    void OnDisable()
    {
        retrocederMainMenu.Disable();
    }

    void Update()
    {
        if (retrocederMainMenu.IsPressed())
        {
            VolverAtras();
        }
    }


    /// <summary>
    /// Vuelve al menú principal y oculta los slots.
    /// </summary>
    private void VolverAtras()
    {
        mainMenu.SetActive(true);
        otro.SetActive(false);
    }
}