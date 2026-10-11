using UnityEngine;

/// <summary>
/// Clase GuiMovil que gestiona la interfaz gráfica para dispositivos móviles
/// </summary>
public class GuiMovil : MonoBehaviour
{
    void Awake()
    {
        #if UNITY_ANDROID || UNITY_IOS
            gameObject.SetActive(true);
        #else
            gameObject.SetActive(false);
        #endif
    }
}