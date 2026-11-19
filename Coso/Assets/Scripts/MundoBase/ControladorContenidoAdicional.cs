using UnityEngine;

/// <summary>
/// Controla si el juego esta completo o no para mostrar el contenido adicional
/// </summary>
public class ControladorContenidoAdicional : MonoBehaviour
{

    void Start() {
        if (SaveManager.GetInstance().currentSave.checkpoint.level1_8IsDone)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}