using UnityEngine;

/// <summary>
/// Clase GuiLevel que gestiona la interfaz gráfica del nivel, incluyendo la visualización de las estrellas obtenidas.
/// </summary>
public class GuiLevel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Imagen de la estrellas")]
    private GameObject[] estrellas = new GameObject[3];

    [SerializeField]
    [Tooltip("Imagen de la vida extra")]
    private GameObject vidaExtra;

    [SerializeField]
    [Tooltip("Texto monedas")]
    private TMPro.TextMeshProUGUI textoMonedas;

    private LevelManager levelManager;

    


    void Awake()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
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
        if (GeneralManager.GetInstance().EstadoVidaExtra())
        {
            vidaExtra.SetActive(true);
        }
        else
        {
            vidaExtra.SetActive(false);
        }
        textoMonedas.SetText(GeneralManager.GetInstance().GetMonedasAcumuladas().ToString());
    }
}