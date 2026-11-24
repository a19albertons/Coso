using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Clase que gestiona los ajustes de sonido como su obtención desde las player prefs como su aplicación y actualización
/// </summary>
public class AjustesSonido : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider effectsSlider;

    [SerializeField]
    private AudioMixer mixer;

    private string masterMusicKey = "music";
    private string masterEffectsKey = "effects";

    private float masterMusicLevel;
    private float masterEffectsLevel;

    private float masterMusicLevelPrevio;
    private float masterEffectsLevelPrevio;

    void Awake()
    {
        masterMusicLevel = PlayerPrefs.GetFloat(masterMusicKey, 100);
        masterMusicLevelPrevio = masterMusicLevel;
        musicSlider.value = masterMusicLevel;

        masterEffectsLevel = PlayerPrefs.GetFloat(masterEffectsKey, 50);
        masterEffectsLevelPrevio = masterEffectsLevel;
        effectsSlider.value = masterEffectsLevel;
    }

    void Start()
    {
        mixer.SetFloat("MasterMusic",MathAdditional.SetVolume(masterMusicLevel));
        mixer.SetFloat("MasterEffects",MathAdditional.SetVolume(masterEffectsLevel));
    }

    void Update()
    {
        LeerSlider();
        ActualizarMusic();
        ActualizarEffects();
    }

    /// <summary>
    /// Actualiza los valores internos de los nivels tras leer el valor actual del slider
    /// </summary>
    private void LeerSlider()
    {
        masterMusicLevel = musicSlider.value;
        masterEffectsLevel = effectsSlider.value;
    }

    /// <summary>
    /// Si el slider varia su valor se actualiza en los player prefs y mixer el valor del audio de efectos
    /// </summary>
    private void ActualizarEffects()
    {
        if (!Equals(masterEffectsLevelPrevio, masterEffectsLevel))
        {
            masterEffectsLevelPrevio = masterEffectsLevel;
            PlayerPrefs.SetFloat(masterEffectsKey, masterEffectsLevel);
            mixer.SetFloat("MasterEffects",MathAdditional.SetVolume(masterEffectsLevel));
        }
    }

    /// <summary>
    /// Si el slider varia su valor se actualiza en el player prefs y mixer el valor del audio de musica
    /// </summary>
    private void ActualizarMusic()
    {
        if (!Equals(masterMusicLevelPrevio, masterMusicLevel))
        {
            masterMusicLevelPrevio = masterMusicLevel;
            PlayerPrefs.SetFloat(masterMusicKey, masterMusicLevel);
            mixer.SetFloat("MasterMusic", MathAdditional.SetVolume(masterMusicLevel));
        }
    }
}