using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ControladorVolver controla que puedas volver al menu principal desde la carga del nuevo juego/cargar partida
/// </summary>
public class ControladorVolver : MonoBehaviour
{
    [SerializeField]
    private Button botonVolver;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject otro;


    void Awake()
    {
        botonVolver.onClick.AddListener(VolverAtras);
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