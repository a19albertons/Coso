using UnityEngine;
using UnityEngine.EventSystems;

public class SeleccionarAlPasarElRaton : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}