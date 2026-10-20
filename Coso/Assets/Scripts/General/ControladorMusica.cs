using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase que gestiona la música de fondo del juego, cambiando la música según la escena cargada
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ControladorMusica : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Musica que se reproducirá en el menú inicial")]
    private AudioClip musicaMenuInicial;

    [SerializeField]
    [Tooltip("Musica que se reproducirá en el mundo base")]
    private AudioClip musicaMundoBase;

    [SerializeField]
    [Tooltip("Musica que se reproducirá en el nivel")]
    private AudioClip[] musicaNivel;

    [SerializeField]
    [Tooltip("Musica que se reproducirá en el nivel final")]
    private AudioClip musicaNivelFinal;

    /// <summary>
    /// Instancia única del controlador de música para mantener la música entre escenas
    /// </summary>
    private static ControladorMusica instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Suscribirse al evento de carga de escena para cambiar la música según la escena cargada
            SceneManager.sceneLoaded += OnSceneLoaded;
            audioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Se llama cada vez que se carga una nueva escena. Cambia la música según la escena cargada.
    /// </summary>
    /// <param name="scene">Escena a cargar</param>
    /// <param name="modo">modo de cargar</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode modo)
    {
        AudioClip musicaAReproducir = scene.name switch
        {
            "NewGame" => musicaMenuInicial,
            "MundoBase" => musicaMundoBase,
            "1-8" => musicaNivelFinal,
            "1-1" or "1-2" or "1-3" or "1-4" or "1-5" or "1-6" or "1-7" => musicaNivel[Random.Range(0, musicaNivel.Length)],
            _ => null,
        };

        // Evalua si hay musica a reproducir, si no la hay detiene el audio source
        if (musicaAReproducir != null)
        {
            audioSource.clip = musicaAReproducir;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

    }

    /// <summary>
    /// Se llama cuando el objeto es destruido. Se desuscribe del evento de carga de escena para evitar errores al destruir el objeto.
    /// </summary>
    void OnDestroy()
    {
        if (instance == this)
        {
            // Desuscribirse del evento de carga de escena al destruir el objeto
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }

    }
}