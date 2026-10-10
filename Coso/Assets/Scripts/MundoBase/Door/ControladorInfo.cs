using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ControladorDoor))]
public class ControladorInfo : MonoBehaviour
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

    private ControladorDoor controladorDoor;

    void Awake()
    {
        controladorDoor = GetComponent<ControladorDoor>();
        if (SaveManager.GetIntance().currentSave == null)
        {
            Debug.LogError("No se ha cargado ningún archivo de guardado. Asegúrate de que el SaveManager esté inicializado correctamente.");
            return;
        }
        SaveManager.GetIntance().currentSave.levels.ForEach(level =>
        {
            for (int i = 0; i < level.starsObtained.Length; i++)
            {
                if (!level.identificadorNivel.Equals(controladorDoor.sceneName))
                {
                    continue;
                }
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

        tituloNivel.text = controladorDoor.sceneName;
        estrellasRequeridas.text = controladorDoor.requiredStars.ToString();
    }
}