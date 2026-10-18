using TMPro;
using UnityEngine;

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