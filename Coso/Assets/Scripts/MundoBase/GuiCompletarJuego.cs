using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Clase que se encarga de gestionar el canvas de cuando completas el juego
/// </summary>
public class GuiCompletarJuego : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Boton de ok que cierra la ventana")]
    private Button botonOk;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(botonOk.gameObject);
        botonOk.onClick.AddListener(ClickarOk);
    }

    /// <summary>
    /// Gestiona el evento de click del boton Ok, cierra la ventana.
    /// </summary>
    private void ClickarOk()
    {
        gameObject.SetActive(false);
    }
}