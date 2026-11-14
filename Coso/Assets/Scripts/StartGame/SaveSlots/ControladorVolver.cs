using System;
using UnityEngine;
using UnityEngine.UI;

public class ControladorVolver : MonoBehaviour
{
    [SerializeField]
    private Button botonVolver;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject slots;


    void Awake()
    {
        botonVolver.onClick.AddListener(VolverAtras);
    }

    private void VolverAtras()
    {
        mainMenu.SetActive(true);
        slots.SetActive(false);
    }
}