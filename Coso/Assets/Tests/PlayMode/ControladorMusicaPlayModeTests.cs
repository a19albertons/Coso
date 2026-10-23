using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Clase de pruebas unitarias para la clase ControladorMusica en modo PlayMode. Estas pruebas verifican el comportamiento de la gestión de música durante la ejecución del juego.
/// </summary>
public class ControladorMusicaPlayModeTests
{
    private const string testSceneName = "Tests";

    /// <summary>
    /// Se ejecuta una vez antes de todas las pruebas en modo PlayMode. Carga la escena de prueba para asegurar que todas las pruebas se realicen en un entorno consistente y controlado.
    /// </summary>
    [UnityOneTimeSetUp]
    public IEnumerator OneTimeSetUp()
    {
        Debug.Log("Escena actual: " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name != testSceneName)
        {
            SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
            yield return null;
        }
    }

    /// <summary>
    /// Se ejecuta antes de cada prueba en modo PlayMode.
    /// </summary>
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        yield return null;
    }

    /// <summary>
    /// Se ejecuta después de cada prueba en modo PlayMode. Destruye los objetos persistentes y resetea los campos estáticos de los singletons.
    /// </summary>
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (ControladorMusica.GetInstance() != null)
            Object.Destroy(ControladorMusica.GetInstance().gameObject);

        yield return null;

        ReflectionHelper.ResetStaticField(typeof(ControladorMusica), "instance");
    }


    // ---- Test 1: Awake --> inicializa audio source -------------
    /// <summary>
    /// Verifica que al crear una instancia de ControladorMusica, se inicialice correctamente el AudioSource.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator Awake_ConSave_InicializaAudioSource()
    {
        var go = new GameObject("ControladorMusica");
        var cm = go.AddComponent<ControladorMusica>();
        yield return null;

        Assert.IsNotNull(ControladorMusica.GetInstance());
        Assert.IsNotNull(cm.GetComponent<AudioSource>());
    }

    // ---- Test 2: segunda instancia se destruye --------------------------------
    /// <summary>
    /// Verifica que al crear una segunda instancia de ControladorMusica, la primera instancia se mantenga y la segunda se destruya, asegurando el comportamiento de singleton.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SegundaInstancia_SeDestruyeYPrevaleceLaPrimera()
    {

        var go1 = new GameObject("G1");
        var cm1 = go1.AddComponent<ControladorMusica>();
        yield return null;

        var go2 = new GameObject("G2");
        go2.AddComponent<ControladorMusica>();
        yield return null;

        Assert.IsTrue(go1 != null, "La primera instancia debe sobrevivir");
        Assert.IsTrue(go2 == null, "La segunda instancia debe haberse destruido");
        Assert.AreSame(cm1, ControladorMusica.GetInstance());
    }

    // ---- Test 3: GetInstance antes de crear el objeto ------------------------
    /// <summary>
    /// Verifica que el método GetInstance devuelva null antes de crear una instancia de ControladorMusica, asegurando que el singleton no esté inicializado.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator GetInstance_AntesDeCrear_DevuelveNull()
    {
        yield return null;
        Assert.IsNull(ControladorMusica.GetInstance());
    }

    // ---- Test 4: DontDestroyOnLoad --> persiste entre escenas -----------------
    /// <summary>
    /// Verifica que la instancia de ControladorMusica persista entre cambios de escena, asegurando que el objeto no se destruya al cargar una nueva escena.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator Musica_Persiste_TrasCambiarDeEscena()
    {
        var go = new GameObject("ControladorMusica");
        var cm = go.AddComponent<ControladorMusica>();
        yield return null;

        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AI/Musica/Apex_of_the_Climb.mp3");
        cm.GetComponent<AudioSource>().clip = clip;
        cm.GetComponent<AudioSource>().Play();

        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;

        Assert.IsNotNull(ControladorMusica.GetInstance());
        Assert.AreSame(cm, ControladorMusica.GetInstance(),
            "DontDestroyOnLoad debe mantener la misma instancia tras cambiar de escena");
        Assert.IsNotNull(cm.GetComponent<AudioSource>());
    }

    // ---- Test 5: OnDestroy desuscribe evento SceneManager.sceneLoaded --------
    /// <summary>
    /// Realiza las comprobaciones del metodo destroy.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    [Ignore("Se tiene que investigar la lógica para saber si se esta subscrito a un evento o no y evaluar si se hacen correctamente las operaciones")]
    public IEnumerator OnDestroy_DesuscribeEventoSceneLoaded()
    {
        // Test no implementado
        return null;
    }

    // ---- Test 6: OnDestroy nullifica el singleton ----------------------------
    /// <summary>
    /// Verifica que al destruir una instancia de ControladorMusica, el singleton se nullifique.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator OnDestroy_NullificaSingleton()
    {
        var go = new GameObject("ControladorMusica");
        var cm = go.AddComponent<ControladorMusica>();
        yield return null;

        Assert.IsNotNull(ControladorMusica.GetInstance());
        Assert.AreSame(cm, ControladorMusica.GetInstance());

        Object.Destroy(go);
        yield return null;

        Assert.IsNull(ControladorMusica.GetInstance());
    }
}
