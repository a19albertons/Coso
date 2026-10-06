using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase GuiLevel que gestiona la interfaz gráfica del nivel, incluyendo la visualización de las estrellas obtenidas.
/// </summary>
public class GuiLevel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Imagen de la estrellas")]
    private GameObject[] estrellas = new GameObject[3];

    [SerializeField]
    [Tooltip("Imagen del powerUP habilitado")]
    private GameObject powerUP;

    [SerializeField]
    [Tooltip("Texto monedas")]
    private TMPro.TextMeshProUGUI textoMonedas;

    [SerializeField]
    [Tooltip("Texto tiempo")]
    private TMPro.TextMeshProUGUI textoTiempo;

    [SerializeField]
    [Tooltip("Texto vidas")]
    private TMPro.TextMeshProUGUI textoVidas;

    [Header("Imagenes de los distintos powerups")]
    [SerializeField]
    [Tooltip("Imagenes de los distintos powerups. La orden debe ser coherente con el código fuente")]
    private Sprite[] powerups;

    private GestionEstrellas levelManager;
    private GestionTiempo tiempoManager;


    void Awake()
    {
        levelManager = FindAnyObjectByType<GestionEstrellas>();
        tiempoManager = FindAnyObjectByType<GestionTiempo>();

    }

    void Update()
    {
        ActualizarGui();
    }

    void ActualizarGui()
    {
        if (levelManager == null)
        {
            Debug.LogWarning("LevelManager no está asignado en GuiLevel.");
            return;
        }
        else
        {
            // Actualizar la visualización de las estrellas según su estado
            for (int i = 0; i < estrellas.Length; i++)
            {
                if (i < levelManager.InformaciónTodasLasEstrellas().Length)
                {
                    estrellas[i].SetActive(levelManager.InformaciónTodasLasEstrellas()[i] == EnumStars.Stars.Obtenida);
                }
            }
        }
        powerUP.GetComponent<Image>().sprite = GestionPowerUP.GetInstance().EstadoPowerUP() switch
        {
            NoneState => powerups[0],
            DoubleJumpState => powerups[1],
            _ => powerups[0],
        };

        textoMonedas.SetText(GestionMonedas.GetInstance().GetMonedasAcumuladas());
        textoTiempo.SetText(tiempoManager.TiempoRestante());
        textoVidas.SetText(GestionVidas.GetInstance().GetVidasActuales());
    }
}