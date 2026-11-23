using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AjustesPantalla : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown tipoScreen;

    [SerializeField]
    private TMP_Dropdown resolucion;

    private int tipoScreenPrevia;
    private int tipoResolucionPrevia;
    Resolution[] listadoResoluciones;

    // Clave para las prefs
    private string tipoScreenKey = "tipoScreen";
    private string resolucionKey = "resolucion";


    void Awake()
    {
        tipoScreen.ClearOptions();
        resolucion.ClearOptions();

        List<string> tipoScreenOptions = new()
        {
            "Pantalla completa",
            "Ventana",
        };


        tipoScreen.AddOptions(tipoScreenOptions);

        List<string> resolucionesString = new();
        // Creamos la lista de resoluciones
        foreach (Resolution resoluciones in Screen.resolutions)
        {
            int divisor = MathAdditional.GetGCD(resoluciones.width, resoluciones.height);
            int widthAspect = resoluciones.width / divisor;
            int heightAspect = resoluciones.height / divisor;

            resolucionesString.Add(resoluciones.width + "x" + resoluciones.height + " (" + widthAspect + ":" + heightAspect + ")");
        }
        // Cacheamos las resoluciones
        listadoResoluciones = Screen.resolutions;
        // Invertimos la lista cacheada para ambos casos (mayor a menor)
        Array.Reverse(listadoResoluciones);
        resolucionesString.Reverse();

        // Añadimos la lista
        resolucion.AddOptions(resolucionesString);

        // Restablecemos el valor en prefs si existe sino como valor por defecto tanto para la variable inicial como la previa
        tipoScreen.value = PlayerPrefs.GetInt(tipoScreenKey, 0);
        tipoScreenPrevia = tipoScreen.value;

        resolucion.value = PlayerPrefs.GetInt(resolucionKey, 0);
        tipoResolucionPrevia = resolucion.value;

    }

    void Update()
    {
        CambiaElModoPantalla();
        HabilitarResolucion();
        AplicarResolucion();


    }

    /// <summary>
    /// Aplica la resolución configurada en caso de estar en modo ventana
    /// </summary>
    private void AplicarResolucion()
    {
        if (!tipoResolucionPrevia.Equals(resolucion.value))
        {
            tipoResolucionPrevia = resolucion.value;
            PlayerPrefs.SetInt(resolucionKey, resolucion.value);
            Screen.SetResolution(
                listadoResoluciones[resolucion.value].width,
                listadoResoluciones[resolucion.value].height,
                false
            );
        }
    }

    /// <summary>
    /// Cambia el modo de pantalla en función de si ha cambiado el tipo o no
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void CambiaElModoPantalla()
    {
        // checkeamos el actual vs el viejo
        if (!tipoScreenPrevia.Equals(tipoScreen.value))
        {
            tipoScreenPrevia = tipoScreen.value;
            PlayerPrefs.SetInt(tipoScreenKey, tipoScreen.value);
            switch (tipoScreen.value)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    Screen.SetResolution(
                        listadoResoluciones[0].width,
                        listadoResoluciones[0].height,
                        true
                    );
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    Screen.SetResolution(
                        listadoResoluciones[resolucion.value].width,
                        listadoResoluciones[resolucion.value].height,
                        false
                    );
                    break;
                default:
                    // Pantalla que asumimos por defecto en caso de default no previsto
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    Screen.SetResolution(
                        listadoResoluciones[0].width,
                        listadoResoluciones[0].height,
                        true
                    );
                    break;
            }
        }
    }

    /// <summary>
    /// Gestiona habilitar la opción de cambiar la resolución cuando tenemos elegido el modo ventana
    /// </summary>
    private void HabilitarResolucion()
    {
        if (tipoScreen.value.Equals(1))
        {
            resolucion.interactable = true;
        }
        else
        {
            resolucion.interactable = false;
        }
    }
}