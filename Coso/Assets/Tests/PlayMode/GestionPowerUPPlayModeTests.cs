using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Clase de pruebas unitarias para la clase GestionPowerUP en modo PlayMode. Estas pruebas verifican el comportamiento de la gestión de power ups durante la ejecución del juego, incluyendo la persistencia entre escenas y la interacción con el SaveManager.
/// </summary>
public class GestionPowerUPPlayModeTests
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
        if (GestionPowerUP.GetInstance() != null)
            Object.Destroy(GestionPowerUP.GetInstance().gameObject);
        if (SaveManager.GetInstance() != null)
            Object.Destroy(SaveManager.GetInstance().gameObject);

        yield return null;

        ReflectionHelper.ResetStaticField(typeof(GestionPowerUP), "instance");
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");

        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;
    }

    // ---- Test 1: Awake sin save → LogError y estado NoneState ----------------
    [UnityTest]
    public IEnumerator Awake_SinSave_CargaCeroYLoggeaError()
    {
        LogAssert.Expect(LogType.Error,
            new Regex("No se ha cargado un archivo de guardado"));

        var go = new GameObject("GestionPowerUP");
        go.AddComponent<GestionPowerUP>();

        yield return null;

        Assert.IsNotNull(GestionPowerUP.GetInstance());
        // El estado si no hay save siempre es nulo nuca entra al else que es quien configura un estado valido (distinto de nulo)
        Assert.IsNull(GestionPowerUP.GetInstance().EstadoPowerUP());
    }

    // ---- Test 2: Awake con save cargado → inicializa power up -----------------
    [UnityTest]
    public IEnumerator Awake_ConSave_InicializaPowerUpDesdeSave()
    {
        var sm = SaveManager.GetInstance();
        sm.currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { powerUpActual = PowerUPType.DoubleJump }
        };

        var go = new GameObject("GestionPowerUP");
        go.AddComponent<GestionPowerUP>();
        yield return null;

        Assert.AreEqual(PowerUPType.DoubleJump, GestionPowerUP.GetInstance().TipoPowerUP());
    }

    // ---- Test 3: segunda instancia se destruye --------------------------------
    [UnityTest]
    public IEnumerator SegundaInstancia_SeDestruyeYPrevaleceLaPrimera()
    {
        SaveManager.GetInstance().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { powerUpActual = PowerUPType.DoubleJump }
        };

        var go1 = new GameObject("G1");
        var gp1 = go1.AddComponent<GestionPowerUP>();
        yield return null;

        var go2 = new GameObject("G2");
        go2.AddComponent<GestionPowerUP>();
        yield return null;

        Assert.IsTrue(go1 != null, "La primera instancia debe sobrevivir");
        Assert.IsTrue(go2 == null, "La segunda instancia debe haberse destruido");
        Assert.AreSame(gp1, GestionPowerUP.GetInstance());
    }

    // ---- Test 4: GetInstance antes de crear el objeto ------------------------
    [UnityTest]
    public IEnumerator GetInstance_AntesDeCrear_DevuelveNull()
    {
        yield return null;
        Assert.IsNull(GestionPowerUP.GetInstance());
    }

    // ---- Test 5: DontDestroyOnLoad → persiste entre escenas ------------------
    [UnityTest]
    public IEnumerator PowerUp_Persiste_TrasCambiarDeEscena()
    {
        SaveManager.GetInstance().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { powerUpActual = PowerUPType.DoubleJump }
        };

        var go = new GameObject("GestionPowerUP");
        var gp = go.AddComponent<GestionPowerUP>();
        yield return null;

        gp.PlayerObtienePowerUP(new DoubleJumpState(new PlayerController()));
        Assert.AreEqual(PowerUPType.DoubleJump, gp.TipoPowerUP());

        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;

        Assert.IsNotNull(GestionPowerUP.GetInstance());
        Assert.AreSame(gp, GestionPowerUP.GetInstance(),
            "DontDestroyOnLoad debe mantener la misma instancia tras cambiar de escena");
        Assert.AreEqual(PowerUPType.DoubleJump, GestionPowerUP.GetInstance().TipoPowerUP());
    }

    // ---- Test 6: PlayerPierdePowerUP -> cambia a NoneState -------------------
    [UnityTest]
    public IEnumerator PlayerPierdePowerUP_CambiaEstadoANoneState()
    {
        SaveManager.GetInstance().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { powerUpActual = PowerUPType.DoubleJump }
        };

        var go = new GameObject("GestionPowerUP");
        var gp = go.AddComponent<GestionPowerUP>();
        yield return null;

        // Pierde el power up y se pasa a NoneState
        GestionPowerUP.GetInstance().PlayerPierdePowerUP();
        yield return null;

        Assert.AreEqual(typeof(NoneState), GestionPowerUP.GetInstance().EstadoPowerUP().GetType());
    }

    // ---- Test 7: PlayerObtienePowerUP -> cambia al tipo especificado ----------
    [UnityTest]
    public IEnumerator PlayerObtienePowerUP_CambiaEstadoAlTipoEspecificado()
    {
        SaveManager.GetInstance().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { powerUpActual = PowerUPType.None }
        };

        var go = new GameObject("GestionPowerUP");
        var gp = go.AddComponent<GestionPowerUP>();
        yield return null;

        // Obtiene un nuevo power up y se cambia al estado correspondiente
        GestionPowerUP.GetInstance().PlayerObtienePowerUP(new DoubleJumpState(new PlayerController()));
        yield return null;

        Assert.AreEqual(typeof(DoubleJumpState), GestionPowerUP.GetInstance().EstadoPowerUP().GetType());
    }

    // ---- Test 9: Awake sin SaveManager → NRE (documenta el bug) -------------
    [UnityTest]
    public IEnumerator Awake_SinSaveManager_LanzaNullReference()
    {
        Object.DestroyImmediate(saveManagerGO);
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");

        LogAssert.Expect(LogType.Exception, new Regex("NullReferenceException"));

        var go = new GameObject("GestionPowerUP");
        go.AddComponent<GestionPowerUP>();
        yield return null;

        Assert.IsNotNull(GestionPowerUP.GetInstance());
    }

    // ---- Test 10: EstadoPowerUP devuelve el estado actual --------------------
    [UnityTest]
    public IEnumerator EstadoPowerUP_DevuelveEstadoActual()
    {
        // Definimos los valores en save y a comaprar
        var doubleJumpStateType = typeof(DoubleJumpState);
        saveManagerGO.GetComponent<SaveManager>().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { powerUpActual = PowerUPType.DoubleJump }
        };
        
        // Creamos el objeto GestionPowerUP y esperamos un frame para que se inicialice
        var go = new GameObject("GestionPowerUP");
        var gp = go.AddComponent<GestionPowerUP>();

        yield return null;

        // Comprobamos que el estado actual es del tipo esperado (DoubleJumpState)
        var resultado = GestionPowerUP.GetInstance().EstadoPowerUP();
        Assert.AreEqual(doubleJumpStateType, resultado.GetType());
    }
}
