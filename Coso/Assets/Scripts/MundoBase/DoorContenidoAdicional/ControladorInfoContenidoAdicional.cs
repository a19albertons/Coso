using TMPro;
using UnityEngine;

/// <summary>
/// Clase controlador que gestiona el nombre de la escena como titulo
/// </summary>
[RequireComponent(typeof(ControladorDoorContenidoAdicional))]
public class ControladorInfoContenidoAdicional : MonoBehaviour
{

    [SerializeField]
    [Tooltip("TextMeshPro del panel info que muestra el título del nivel")]
    private TextMeshProUGUI tituloNivel;

    private ControladorDoorContenidoAdicional controladorDoorNivel;

    void Awake()
    {
        controladorDoorNivel = GetComponent<ControladorDoorContenidoAdicional>();
    }

    void Update()
    {
        ActualizarInfo();
    }

    private void ActualizarInfo()
    {

        tituloNivel.text = controladorDoorNivel.sceneName;
    }
}