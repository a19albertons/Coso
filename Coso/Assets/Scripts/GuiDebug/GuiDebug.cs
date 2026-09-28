using UnityEngine;

public class GuiDebug : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Texto para mostrar el contador de FPS")]
    private TMPro.TextMeshProUGUI contadorFPS;
    private int frameCount = 0;
    private float elapsedTime = 0f;
    private string textoBaseContador;

    void Awake()
    {
        textoBaseContador = contadorFPS.text.Trim();
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
    }
}