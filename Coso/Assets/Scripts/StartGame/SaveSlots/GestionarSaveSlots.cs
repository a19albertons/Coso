using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Gestiona los slots de guardado
/// </summary>
public class GestionarSaveSlots : MonoBehaviour
{
    [SerializeField]
    private GameObject slotConJuego;

    [SerializeField]
    private GameObject slotSinJuego;

    [SerializeField]
    private GameObject contenedor;

    [SerializeField]
    private int numeroSlots;

    [SerializeField]
    private GameObject[] puntosInstanciacion;

    [SerializeField]
    private GameObject[] objectosInstanciados;

    [SerializeField]
    private Color colorGris;

    private bool definido = false;

    private void Awake()
    {
        CargarSlots();

    }

    private void OnEnable()
    {
        if (StartGame.newGame)
        {
            HabilitarSlotsSinJuego();
        }
        else
        {
            DeshabilitarSlotsSinJuego();
        }
    }

    void OnDisable()
    {
        definido = false;
    }

    /// <summary>
    /// Se encarga de instanciar los slots en función de los datos que se detectan guardados
    /// </summary>
    private void CargarSlots()
    {
        for (int i = 0; i < numeroSlots; i++)
        {
            if (File.Exists(Path.Combine(Application.persistentDataPath, "slotGuardado" + (i + 1))))
            {
                GameObject go = Instantiate(slotConJuego, puntosInstanciacion[i].transform.position, Quaternion.identity, contenedor.transform);
                go.GetComponent<ControladorSlot>().ActualizarTexto(i + 1);
                objectosInstanciados[i] = go;
            }
            else
            {
                GameObject go = Instantiate(slotSinJuego, puntosInstanciacion[i].transform.position, Quaternion.identity, contenedor.transform);
                go.GetComponent<ControladorSlot>().ActualizarTexto(i + 1);
                objectosInstanciados[i] = go;
            }
        }
    }

    /// <summary>
    /// Habilita los slots sin guardado asociado en caso de nuevo juego
    /// </summary>
    public void HabilitarSlotsSinJuego()
    {
        for (int i = 0; i < numeroSlots; i++)
        {
            if (objectosInstanciados[i].name.StartsWith("SlotSinJuego"))
            {
                objectosInstanciados[i].GetComponent<Button>().enabled = true;
                objectosInstanciados[i].GetComponent<Image>().color = Color.white;
                if (!definido)
                {
                    EventSystem.current.SetSelectedGameObject(objectosInstanciados[i]);
                    definido = true;
                }
            }
        }
    }

    /// <summary>
    /// Deshabilita los slots sin guardado asociado en caso de cargar juego
    /// </summary>
    public void DeshabilitarSlotsSinJuego()
    {
        for (int i = 0; i < numeroSlots; i++)
        {
            if (objectosInstanciados[i].name.StartsWith("SlotSinJuego"))
            {
                objectosInstanciados[i].GetComponent<Button>().enabled = false;
                objectosInstanciados[i].GetComponent<Image>().color = colorGris;
            }
            else if (!definido)
            {
                EventSystem.current.SetSelectedGameObject(objectosInstanciados[i]);
                definido = true;
            }
        }
    }
}