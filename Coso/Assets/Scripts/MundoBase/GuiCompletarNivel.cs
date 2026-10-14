using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Clase que se encarga de gestionar el canvas de guardado al alcanzar algún checkpoint de nivel como el 1-4
/// </summary>
public class GuiCompletarNivel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Botón que si guardara los cambios y será el marcado por defecto")]
    private Button botonSi;

    [SerializeField]
    [Tooltip("Boton que no guardara los cambios de forma permanente y cierra la ventana")]
    private Button botonNo;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(botonSi.gameObject);
        botonSi.onClick.AddListener(ClickarSi);
        botonNo.onClick.AddListener(ClickarNo);
    }

    private void ClickarNo()
    {
        GuardarProgreso();
        gameObject.SetActive(false);
    }

    private void ClickarSi()
    {
        ConsolidarProgreso();
        GuardarProgreso();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Realiza los cambios de estado provisional a obtenido de forma permanente
    /// </summary>
    private void ConsolidarProgreso()
    {
        if (SaveManager.GetIntance().currentSave == null)
        {
            Debug.LogError("No hay un guardado activo para aplicar el progreso del nivel");
            return;
        }
        // Se recorre la lista de niveles del guardado actual y se actualiza el estado de los niveles y estrellas obtenidas
        SaveManager.GetIntance().currentSave.levels.ForEach(level =>
        {
            // Guardar de forma permanente el progreso obtenido
            if (level.isLevelCompleted == EnumStateLevelYStars.LevelYStars.Provisional)
            {
                level.isLevelCompleted = EnumStateLevelYStars.LevelYStars.Obtenida;
            }
            // Actualizar el estado de las estrellas
            for (int i = 0; i < level.starsObtained.Length; i++)
            {
                if (level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Provisional)
                {
                    level.starsObtained[i] = EnumStateLevelYStars.LevelYStars.Obtenida;
                }
            }
        });
    }

    /// <summary>
    /// Se guarda el progreso en el fichero asociado en el sistema de ficheros.
    /// </summary>
    private void GuardarProgreso()
    {
        if (SaveManager.GetIntance().currentSave == null)
        {
            Debug.LogError("No hay un guardado activo para aplicar el progreso del nivel");
            return;
        }
        SaveManager.GetIntance().SaveGame(1);
    }
}