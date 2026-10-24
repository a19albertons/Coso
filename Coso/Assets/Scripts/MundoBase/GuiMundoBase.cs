using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase GuiMundoBase que gestiona la interfaz gráfica del mundobase, incluyendo la visualización de las estrellas obtenidas.
/// </summary>
public class GuiMundoBase : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Imagen del powerUP habilitado")]
    private GameObject powerUP;

    [SerializeField]
    [Tooltip("Texto monedas")]
    private TMPro.TextMeshProUGUI textoMonedas;

    [SerializeField]
    [Tooltip("Texto tiempo")]
    private TMPro.TextMeshProUGUI textoEstrellas;

    [SerializeField]
    [Tooltip("Texto vidas")]
    private TMPro.TextMeshProUGUI textoVidas;

    [Header("Imagenes de los distintos powerups")]
    [SerializeField]
    [Tooltip("Imagenes de los distintos powerups. La orden debe ser coherente con el código fuente")]
    private Sprite[] powerups;


    void Update()
    {
        ActualizarGui();
    }

    /// <summary>
    /// Actualiza la interfaz gráfica del mundobase, incluyendo la imagen del powerUP habilitado, el texto de monedas, el texto de estrellas y el texto de vidas.
    /// </summary>
    void ActualizarGui()
    {
        powerUP.GetComponent<Image>().sprite = GestionPowerUP.GetInstance().EstadoPowerUP() switch
        {
            NoneState => powerups[0],
            DoubleJumpState => powerups[1],
            _ => powerups[0],
        };
        int cantidadEstrellas = 0;
        SaveManager.GetInstance().currentSave.levels.ForEach(level =>
        {
            for (int i = 0; i < level.starsObtained.Length; i++)
            {
                if (level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Obtenida || level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Provisional)
                {
                    cantidadEstrellas++;
                }
            }
        }
        );

        textoMonedas.SetText(GestionMonedas.GetInstance().GetMonedasAcumuladas());
        textoEstrellas.SetText(cantidadEstrellas.ToString());
        textoVidas.SetText(GestionVidas.GetInstance().GetVidasActuales());
    }
}