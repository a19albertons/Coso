using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Controla los slots generados controlando comportamiento como el id asignado y carga o creación del guardado del juego
/// </summary>
[RequireComponent(typeof(Button))]
public class ControladorSlot : MonoBehaviour
{
    /// <summary>
    /// Gestiona el slot por posición que se le pasa al forzar el texto en su generación
    /// </summary>
    public int numeroSlot {get; private set;}

    [SerializeField]
    private TextMeshProUGUI textoModificar;

    Button boton;

    void Awake()
    {
        boton = GetComponent<Button>();
    }

    /// <summary>
    /// Metodo que define el texto de numero del slot y guarda su numero de slot internamente cuando se instancia
    /// </summary>
    /// <param name="slot"></param>
    public void ActualizarTexto(int slot)
    {
        numeroSlot = slot;
        textoModificar.text = "Slot "+slot;
    }

    void OnEnable()
    {
        if (StartGame.newGame)
        {
            boton.onClick.AddListener(NuevoJuego);
        }
        else
        {
            boton.onClick.AddListener(CargarJuego);
        }
    }

    void OnDisable()
    {
        boton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Gestiona la inicialización del juego
    /// </summary>
    void NuevoJuego()
    {
        SaveManager.GetInstance().NewGame(numeroSlot);
        SceneManager.LoadScene("Intro");
    }

    /// <summary>
    /// Gestiona la carga de un slot
    /// </summary>
    void CargarJuego()
    {
        SaveManager.GetInstance().LoadGame(numeroSlot);
        SceneManager.LoadScene("MundoBase");
    }
}