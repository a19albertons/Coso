using UnityEngine;
using UnityEngine.InputSystem;

public class GestionIteracciones : MonoBehaviour
{
    [Header("Configuración de Input")]
    [SerializeField]
    [Tooltip("Set de movimientos del jugador")]
    private InputActionAsset inputActions;
    private InputActionMap playerActionMap;
    private InputAction EntrarAlNivel;

    /// <summary>
    /// Busca el boton de iteraciones
    /// </summary>
    void Awake()
    {
        playerActionMap = inputActions.FindActionMap("Gameplay");
        EntrarAlNivel = playerActionMap.FindAction("Puerta");
    }
    
    /// <summary>
    /// Se llama cuando el objeto se activa. Habilita la acción de entrar al nivel para que pueda ser detectada mientras el objeto está activo.
    /// </summary>
    void OnEnable()
    {
        EntrarAlNivel.Enable();
    }

    /// <summary>
    /// Se llama cuando el objeto se desactiva. Deshabilita la acción de entrar al nivel para evitar que se siga ejecutando mientras el objeto está desactivado.
    /// </summary>
    void OnDisable()
    {
        EntrarAlNivel.Disable();
    }
}