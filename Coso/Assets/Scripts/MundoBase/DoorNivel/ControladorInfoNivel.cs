using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase controlador que gestiona el nombre de la escena como titulo
/// </summary>
[RequireComponent(typeof(ControladorDoorNivel))]
public class ControladorInfoNivel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Las estrellas del panel info en orden correcta izquierda a derecha")]
    private GameObject[] stars;

    [SerializeField]
    [Tooltip("TextMeshPro del panel info que muestra el título del nivel")]
    private TextMeshProUGUI tituloNivel;

    [SerializeField]
    [Tooltip("TextMeshPro que indica la cantidad de estrellas requeridas")]
    private TextMeshProUGUI estrellasRequeridas;

    [Header("Configuración imágenes de estrellas")]
    [SerializeField]
    [Tooltip("Sprite de estrella obtenida")]
    private Sprite estrellaObtenida;

    [SerializeField]
    [Tooltip("Sprite de estrella no obtenida")]
    private Sprite estrellaNoObtenida;

    private ControladorDoorNivel controladorDoorNivel;

    void Awake()
    {
        controladorDoorNivel = GetComponent<ControladorDoorNivel>();
        if (SaveManager.GetInstance().currentSave == null)
        {
            Debug.LogError("No se ha cargado ningún archivo de guardado. Asegúrate de que el SaveManager esté inicializado correctamente.");
        }
    }

    void Update()
    {
        ActualizarInfo();
    }

    private void ActualizarInfo()
    {
        SaveManager.GetInstance().currentSave.levels.ForEach(level =>
        {

            if (level.identificadorNivel.Equals(controladorDoorNivel.sceneName))
            {
                for (int j = 0; j < level.starsObtained.Length; j++)
                {
                    if (level.starsObtained[j] == EnumStateLevelYStars.LevelYStars.Obtenida || level.starsObtained[j] == EnumStateLevelYStars.LevelYStars.Provisional)
                    {
                        stars[j].GetComponent<Image>().sprite = estrellaObtenida;
                    }
                    else
                    {
                        stars[j].GetComponent<Image>().sprite = estrellaNoObtenida;
                    }
                }
            }


        });

        tituloNivel.text = controladorDoorNivel.sceneName;
        estrellasRequeridas.text = controladorDoorNivel.requiredStars.ToString();
    }
}