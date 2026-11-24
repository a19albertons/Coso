using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AjustesFPS : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown cantidadFPSObjetivo;

    private string fpsKey = "fps";
    private int cantidadFPSObjetivoPrevio;


    void Awake()
    {
        cantidadFPSObjetivo.ClearOptions();

        List<string> listaFPS = new() { "monitor", "24", "30", "60", "120", "144", "unlimited" };
        cantidadFPSObjetivo.AddOptions(listaFPS);

        cantidadFPSObjetivo.value = PlayerPrefs.GetInt(fpsKey, 0);
        cantidadFPSObjetivoPrevio = cantidadFPSObjetivo.value;

        AplicarFPS();
    }

    void Update()
    {
        ValidarAplicacion();
    }

    private void ValidarAplicacion()
    {
        if (!Equals(cantidadFPSObjetivo.value, cantidadFPSObjetivoPrevio))
        {
            cantidadFPSObjetivoPrevio = cantidadFPSObjetivo.value;
            PlayerPrefs.SetInt(fpsKey, cantidadFPSObjetivo.value);
            AplicarFPS();
        }
    }

    /// <summary>
    /// Aplica el valor de fps cuando es llamada en función del valor del dropdown
    /// </summary>
    private void AplicarFPS()
    {
        switch (cantidadFPSObjetivo.value)
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
                break;

            case 1:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 24;
                break;
            case 2:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 30;
                break;
            case 3:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
                break;
            case 4:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 120;
                break;
            case 5:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 144;
                break;
            case 6:
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = -1;
                break;
            default:
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
                break;
        }
    }
}