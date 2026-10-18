using UnityEngine;

/// <summary>
/// Clase que gestiona la información de debug en las distintas escenas como contador de fps e información del player en su movimiento por el eje X e Y
/// </summary>
public class GuiDebug : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Cuadro de texto para mostrar el contador de FPS")]
    private TMPro.TextMeshProUGUI contadorFPS;

    [SerializeField]
    [Tooltip("Cuadro de texto para mostrar la información de X")]
    private TMPro.TextMeshProUGUI infoX;

    [SerializeField]
    [Tooltip("Cuadro de texto para mostrar la información de Y")]
    private TMPro.TextMeshProUGUI infoY;

    [SerializeField]
    [Tooltip("Objeto de donde obtener la información de X e Y")]
    private Rigidbody2D RigidbodyInfo;

    private int frameCount = 0;
    private float elapsedTime = 0f;
    private string textoBaseContador;
    private string textoBaseInfoX;
    private string textoBaseInfoY;

    void Awake()
    {
        textoBaseContador = contadorFPS.text.Trim();
        textoBaseInfoX = infoX.text.Trim();
        textoBaseInfoY = infoY.text.Trim();

    }

    void Update()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1f)
        {
            contadorFPS.text = textoBaseContador + " " + frameCount.ToString();
            frameCount = 0;
            elapsedTime -= 1f;
        }
        if (RigidbodyInfo == null)
        {
            Debug.LogWarning("RigidbodyInfo no está asignado en GuiDebug.");
            return;
        }
        else
        {
            infoX.text = textoBaseInfoX + " " + RigidbodyInfo.linearVelocityX.ToString("F2");
            infoY.text = textoBaseInfoY + " " + RigidbodyInfo.linearVelocityY.ToString("F2");
        }
    }
}