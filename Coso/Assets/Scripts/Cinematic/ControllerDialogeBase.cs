using TMPro;
using UnityEngine;

/// <summary>
/// Clase base que gestiona el diálogo en las distintas cinemáticas de forma estandarizada como si fuese una plantilla.
/// </summary>
public abstract class ControllerDialogeBase : MonoBehaviour
{
    protected float time;
    protected bool tiempoIniciado = false;

    protected TextMeshProUGUI dialogo;
    void Awake()
    {
        dialogo = GetComponentInChildren<TextMeshProUGUI>();
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