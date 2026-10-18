using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Clase que gestiona la selección de un botón al pasar el ratón por encima de él
/// </summary>
public class SeleccionarAlPasarElRaton : MonoBehaviour, IPointerEnterHandler
{
    /// <summary>
    /// Gestiona el evento de pasar el ratón por encima del botón, seleccionando el botón en el EventSystem
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}