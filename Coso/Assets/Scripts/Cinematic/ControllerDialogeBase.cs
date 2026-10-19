using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

/// <summary>
/// Clase base que gestiona el diálogo en las distintas cinemáticas de forma estandarizada como si fuese una plantilla.
/// </summary>
public abstract class ControllerDialogeBase : MonoBehaviour
{
    protected float time;
    protected bool tiempoIniciado = false;

    protected TextMeshProUGUI dialogo;
    protected LocalizeStringEvent localize;

    [SerializeField]
    [Tooltip("nombre de la tabla a cargar para el diálogo")]
    protected string tablaNombre;

    void Awake()
    {
        dialogo = GetComponentInChildren<TextMeshProUGUI>();
        localize = GetComponentInChildren<LocalizeStringEvent>();
        if (dialogo == null)
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI en los hijos del objeto.");
        }
        if (localize == null)
        {
            Debug.LogError("No se encontró el componente LocalizeStringEvent en los hijos del objeto.");
        }
        InicializarDialogoMostrado();
    }

    public void IniciarTiempo()
    {
        time = 0f;
        tiempoIniciado = true;
    }

    void Update()
    {
        Dialogo();
    }

    protected abstract void InicializarDialogoMostrado();
    protected abstract void Dialogo();
}