using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Clase de pruebas unitarias para la clase GestionVidas en modo PlayMode. Estas pruebas verifican el comportamiento de la gestión de vidas durante la ejecución del juego, incluyendo la persistencia entre escenas y la interacción con el SaveManager.
/// </summary>
public class GestionVidasPlayModeTests
{
    private const string testSceneName = "Tests";
    private GameObject saveManagerGO;

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
    /// Se ejecuta antes de cada prueba en modo PlayMode. Crea un nuevo GameObject para el SaveManager y lo inicializa.
    /// </summary>
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        saveManagerGO = new GameObject("SaveManager");
        saveManagerGO.AddComponent<SaveManager>();
        yield return null;
    }

    /// <summary>
    /// Se ejecuta después de cada prueba en modo PlayMode. Destruye los objetos persistentes y resetea los campos estáticos de los singletons.
    /// </summary>
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (GestionVidas.GetInstance() != null)
            Object.Destroy(GestionVidas.GetInstance().gameObject);
        if (SaveManager.GetIntance() != null)
            Object.Destroy(SaveManager.GetIntance().gameObject);

        yield return null;

        ResetStaticField(typeof(GestionVidas), "instance");
        ResetStaticField(typeof(SaveManager), "instance");
    }

    /// <summary>
    /// Resetea un campo estático privado de una clase utilizando reflexión.
    /// </summary>
    private static void ResetStaticField(System.Type t, string name)
    {
        var f = t.GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
        if (f != null) f.SetValue(null, null);
    }

    // ---- Test 1: Awake sin save → LogError y vidas 5 --------------------------
    [UnityTest]
    public IEnumerator Awake_SinSave_CargaCeroYLoggeaError()
    {
        LogAssert.Expect(LogType.Error,
            new Regex("No se ha cargado un archivo de guardado"));

        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();

        yield return null;

        Assert.IsNotNull(GestionVidas.GetInstance());
        // Si no hay un valor definido (null) equivale a 0, ya que es un int.
        Assert.AreEqual(0, GestionVidas.GetInstance().GetVidasActualesInt());
        Assert.AreEqual("0", GestionVidas.GetInstance().GetVidasActuales());
    }

    // ---- Test 2: Awake con save cargado --> inicializa vidas ------------------
    [UnityTest]
    public IEnumerator Awake_ConSave_InicializaVidasDesdeSave()
    {
        var sm = SaveManager.GetIntance();
        sm.currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 3 }
        };

        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();
        yield return null;

        Assert.AreEqual(3, GestionVidas.GetInstance().GetVidasActualesInt());
        Assert.AreEqual("3", GestionVidas.GetInstance().GetVidasActuales());
    }

    // ---- Test 3: segunda instancia se destruye --------------------------------
    [UnityTest]
    public IEnumerator SegundaInstancia_SeDestruyeYPrevaleceLaPrimera()
    {
        SaveManager.GetIntance().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 3 }
        };

        var go1 = new GameObject("G1");
        var gv1 = go1.AddComponent<GestionVidas>();
        yield return null;

        var go2 = new GameObject("G2");
        go2.AddComponent<GestionVidas>();
        yield return null;

        Assert.IsTrue(go1 != null, "La primera instancia debe sobrevivir");
        Assert.IsTrue(go2 == null, "La segunda instancia debe haberse destruido");
        Assert.AreSame(gv1, GestionVidas.GetInstance());
    }

    // ---- Test 4: GetInstance antes de crear el objeto ------------------------
    [UnityTest]
    public IEnumerator GetInstance_AntesDeCrear_DevuelveNull()
    {
        yield return null;
        Assert.IsNull(GestionVidas.GetInstance());
    }

    // ---- Test 5: DontDestroyOnLoad --> persiste entre escenas -----------------
    [UnityTest]
    public IEnumerator Vidas_Persisten_TrasCambiarDeEscena()
    {
        SaveManager.GetIntance().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 3 }
        };

        var go = new GameObject("GestionVidas");
        var gv = go.AddComponent<GestionVidas>();
        yield return null;

        gv.PlayerPierdeVida();
        gv.PlayerPierdeVida();
        Assert.AreEqual(1, gv.GetVidasActualesInt());

        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;

        Assert.IsNotNull(GestionVidas.GetInstance());
        Assert.AreSame(gv, GestionVidas.GetInstance(),
            "DontDestroyOnLoad debe mantener la misma instancia tras cambiar de escena");
        Assert.AreEqual(1, GestionVidas.GetInstance().GetVidasActualesInt());
    }

    // ---- Test 6: Awake sin SaveManager --> NRE (documenta el bug) -------------
    [UnityTest]
    public IEnumerator Awake_SinSaveManager_LanzaNullReference()
    {
        Object.DestroyImmediate(saveManagerGO);
        ResetStaticField(typeof(SaveManager), "instance");

        LogAssert.Expect(LogType.Exception, new Regex("NullReferenceException"));

        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();
        yield return null;

        Assert.IsNotNull(GestionVidas.GetInstance());
        // Si el save es null get vidas actuales siempre es 0 en su valor por defecto, ya que es un int.
        Assert.AreEqual(0, GestionVidas.GetInstance().GetVidasActualesInt());
    }

    // ---- Test 7: PlayerPierdeVida reduce vidas en uno ------------------------
    [UnityTest]
    public IEnumerator PlayerPierdeVida_ReduceVidasEnUno()
    {
        // Damos valor al save para que el Awake de GestionVidas inicialice las vidas actuales correctamente.
        SaveManager.GetIntance().currentSave = new CurrentSave()
        {
            playerInfo = new PlayerInfo { vidasActuales = 5 }
        };

        // Fuerza el awake de GestionVidas para inicializar la instancia y las vidas actuales.
        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();
        yield return null;

        var gv = GestionVidas.GetInstance();
        gv.PlayerPierdeVida();
        Assert.AreEqual(4, gv.GetVidasActualesInt());
    }

    // ---- Test 8: PlayerPierdeVida no reduce cuando son 0 --------------------
    [UnityTest]
    public IEnumerator PlayerPierdeVida_CuandoVidasCero_NoReduce()
    {
        // Damos valor al save para que el Awake de GestionVidas inicialice las vidas actuales correctamente.
        SaveManager.GetIntance().currentSave = new CurrentSave()
        {
            playerInfo = new PlayerInfo { vidasActuales = 0 }
        };

        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();
        yield return null;

        var gv = GestionVidas.GetInstance();
        gv.PlayerPierdeVida();
        Assert.AreEqual(0, gv.GetVidasActualesInt());
    }

    // ---- Test 9: PlayerObtieneVida aumenta vidas en uno --------------------
    [UnityTest]
    public IEnumerator PlayerObtieneVida_AumentaVidasEnUno()
    {
        // Damos valor al save para que el Awake de GestionVidas inicialice las vidas actuales correctamente.
        SaveManager.GetIntance().currentSave = new CurrentSave()
        {
            playerInfo = new PlayerInfo { vidasActuales = 5 }
        };

        // Fuerza el awake de GestionVidas para inicializar la instancia y las vidas actuales.
        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();
        yield return null;

        var gv = GestionVidas.GetInstance();
        gv.PlayerObtieneVida();
        Assert.AreEqual(6, gv.GetVidasActualesInt());
    }

    // ---- Test 10: ResetVidas restaura vidas al valor inicial ------------------
    [UnityTest]
    public IEnumerator ResetVidas_RestauraVidasAlValorInicial()
    {
        // Damos valor al save para que el Awake de GestionVidas inicialice las vidas actuales correctamente.
        SaveManager.GetIntance().currentSave = new CurrentSave()
        {
            playerInfo = new PlayerInfo { vidasActuales = 0 }
        };

        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();
        yield return null;

        var gv = GestionVidas.GetInstance();
        gv.ResetVidas();
        Assert.AreEqual(5, gv.GetVidasActualesInt());
    }

    // ---- Test 11: GetVidasActuales devuelve string ---------------------------
    [UnityTest]
    public IEnumerator GetVidasActuales_DevuelveStringCorrecto()
    {
        // Damos valor al save para que el Awake de GestionVidas inicialice las vidas actuales correctamente.
        SaveManager.GetIntance().currentSave = new CurrentSave()
        {
            playerInfo = new PlayerInfo { vidasActuales = 6 }
        };

        // Fuerza el awake de GestionVidas para inicializar la instancia y las vidas actuales.
        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();
        yield return null;
        var gv = GestionVidas.GetInstance();
        Assert.AreEqual("6", gv.GetVidasActuales());
    }

    // ---- Test 12: GetVidasActualesInt devuelve entero ------------------------
    [UnityTest]
    public IEnumerator GetVidasActualesInt_DevuelveEnteroCorrecto()
    {
        // Damos valor al save para que el Awake de GestionVidas inicialice las vidas actuales correctamente.
        SaveManager.GetIntance().currentSave = new CurrentSave()
        {
            playerInfo = new PlayerInfo { vidasActuales = 6 }
        };

        // Fuerza el awake de GestionVidas para inicializar la instancia y las vidas actuales.
        var go = new GameObject("GestionVidas");
        go.AddComponent<GestionVidas>();
        yield return null;

        var gv = GestionVidas.GetInstance();
        Assert.AreEqual(6, gv.GetVidasActualesInt());
    }
}
