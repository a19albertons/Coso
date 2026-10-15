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

    /// <summary>
    /// Gestiona el evento de click del boton No, guarda el evento donde esta ventana ha sucedido y cierra la ventana.
    /// </summary>
    private void ClickarNo()
    {
        GuardadoCentralizadoMundoBase.GuardarProgreso();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Gestiona el evento de click del boton Si, guarda el progreso de forma permanente y cierra la ventana.
    /// </summary>
    private void ClickarSi()
    {
        GuardadoCentralizadoMundoBase.ConsolidarProgreso();
        GuardadoCentralizadoMundoBase.GuardarProgreso();
        gameObject.SetActive(false);
    }


}