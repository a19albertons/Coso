using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Clase de pruebas unitarias para la clase GestionMonedas en modo PlayMode. Estas pruebas verifican el comportamiento de la gestión de monedas durante la ejecución del juego, incluyendo la persistencia entre escenas y la interacción con el SaveManager.
/// </summary>
public class GestionMonedasPlayModeTests
{
    // Escena de prueba que ya tenemos donde se probo el movimiento del juego antes de hacer los niveles oficiales
    private const string testSceneName = "Tests";
    private GameObject saveManagerGO;

    /// <summary>
    /// Se ejecuta una vez antes de todas las pruebas en modo PlayMode. Carga la escena de prueba para asegurar que todas las pruebas se realicen en un entorno consistente y controlado, evitando interferencias de otras escenas o estados del juego.
    /// </summary>
    /// <returns></returns>
    [UnityOneTimeSetUp]
    public IEnumerator OneTimeSetUp()
    {
        Debug.Log("Escena actual: " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name != testSceneName)
        {
            SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
            yield return null; // Espera un frame para asegurarse de que la escena se haya cargado completamente
        }
    }

    /// <summary>
    /// Se ejecuta antes de cada prueba en modo PlayMode. Crea un nuevo GameObject para el SaveManager y lo inicializa, asegurando que el sistema de guardado esté disponible para las pruebas de gestión de monedas.
    /// </summary>
    /// <returns></returns>
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // SaveManager primero: GestionMonedas.Awake lo necesita en su primera línea
        saveManagerGO = new GameObject("SaveManager");
        saveManagerGO.AddComponent<SaveManager>();
        yield return null;
    }

    /// <summary>
    /// Se ejecuta después de cada prueba en modo PlayMode. Destruye el GameObject del SaveManager y resetea los campos estáticos de los singletons para evitar efectos colaterales entre pruebas.
    /// </summary>
    /// <returns></returns>
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Destruir objetos persistentes
        if (GestionMonedas.GetInstance() != null)
            Object.Destroy(GestionMonedas.GetInstance().gameObject);
        if (SaveManager.GetIntance() != null)
            Object.Destroy(SaveManager.GetIntance().gameObject);

        yield return null;

        // Resetear singletons estáticos (los campos son privados estáticos)
        ReflectionHelper.ResetStaticField(typeof(GestionMonedas), "instance");
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");
    }

    // ---- Test 1: Awake sin save → LogError y monedas 0 -------------------------
    /// <summary>
    /// Verifica que al instanciar GestionMonedas sin un archivo de guardado cargado, se registre un error en el log y las monedas acumuladas se inicialicen a cero. Esto asegura que el sistema maneje correctamente la ausencia de datos de guardado y no cause errores inesperados durante la ejecución del juego.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator Awake_SinSave_CargaCeroYLoggeaError()
    {
        // currentSave es null en este punto (SaveManager recién creado, sin cargar save)
        LogAssert.Expect(LogType.Error,
            new Regex("No se ha cargado un archivo de guardado"));

        var go = new GameObject("GestionMonedas");
        go.AddComponent<GestionMonedas>(); // Awake corre aquí mismo, de forma síncrona

        yield return null;

        Assert.IsNotNull(GestionMonedas.GetInstance());
        Assert.AreEqual(0, GestionMonedas.GetInstance().GetMonedasAcumuladasInt());
        Assert.AreEqual("0", GestionMonedas.GetInstance().GetMonedasAcumuladas());
    }

    // ---- Test 2: Awake con save cargado → inicializa monedas -------------------
    /// <summary>
    /// Verifica que al instanciar GestionMonedas con un archivo de guardado cargado, las monedas acumuladas se inicialicen correctamente desde el archivo de guardado. Esto asegura que el sistema de gestión de monedas refleje con precisión el estado del jugador según los datos guardados, proporcionando una experiencia de juego consistente y confiable.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator Awake_ConSave_InicializaMonedasDesdeSave()
    {
        // Crea un archivo de guardado con 50 monedas
        var sm = SaveManager.GetIntance();
        sm.currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { monedasActuales = 50 }
        };

        var go = new GameObject("GestionMonedas");
        go.AddComponent<GestionMonedas>();
        yield return null;

        Assert.AreEqual(50, GestionMonedas.GetInstance().GetMonedasAcumuladasInt());
        Assert.AreEqual("50", GestionMonedas.GetInstance().GetMonedasAcumuladas());
    }

    // ---- Test 3: segunda instancia se destruye --------------------------------
    /// <summary>
    /// Verifica que al crear una segunda instancia de GestionMonedas, esta se destruya automáticamente y la primera instancia prevalezca. Esto asegura que el patrón singleton funcione correctamente, evitando la existencia de múltiples instancias del gestor de monedas y garantizando un comportamiento consistente en la gestión de monedas durante la ejecución del juego.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator SegundaInstancia_SeDestruyeYPrevaleceLaPrimera()
    {
        // Evita el LogError del primer Awake (no afecta al test)
        SaveManager.GetIntance().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { monedasActuales = 0 }
        };

        var go1 = new GameObject("G1");
        var gm1 = go1.AddComponent<GestionMonedas>();
        yield return null;

        var go2 = new GameObject("G2");
        go2.AddComponent<GestionMonedas>();
        yield return null; // aquí se procesa Destroy(go2)

        Assert.IsTrue(go1 != null, "La primera instancia debe sobrevivir");
        Assert.IsTrue(go2 == null, "La segunda instancia debe haberse destruido");
        Assert.AreSame(gm1, GestionMonedas.GetInstance());
    }

    // ---- Test 4: GetInstance antes de crear el objeto ------------------------
    /// <summary>
    /// Verifica que el método GetInstance de GestionMonedas devuelva null antes de que se cree cualquier instancia del objeto. Esto asegura que el patrón singleton funcione correctamente, indicando que no hay ninguna instancia activa del gestor de monedas antes de su creación, lo que es crucial para evitar referencias nulas y errores en tiempo de ejecución.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator GetInstance_AntesDeCrear_DevuelveNull()
    {
        yield return null;
        Assert.IsNull(GestionMonedas.GetInstance());
    }

    // ---- Test 5: DontDestroyOnLoad → persiste entre escenas ------------------
    /// <summary>
    /// Verifica que la instancia de GestionMonedas persista entre cambios de escena, manteniendo el mismo estado de monedas acumuladas. Esto asegura que el sistema de gestión de monedas funcione correctamente a lo largo de diferentes niveles del juego, proporcionando una experiencia de juego continua y consistente para el jugador.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator Monedas_Persisten_TrasCambiarDeEscena()
    {
        SaveManager.GetIntance().currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { monedasActuales = 0 }
        };

        var go = new GameObject("GestionMonedas");
        var gm = go.AddComponent<GestionMonedas>();
        yield return null;

        gm.SumarMoneda();
        gm.SumarMoneda();
        Assert.AreEqual(2, gm.GetMonedasAcumuladasInt());

        // Cargamos la escena de prueba para asegurar que usamos siempre la misma
        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;

        Assert.IsNotNull(GestionMonedas.GetInstance());
        Assert.AreSame(gm, GestionMonedas.GetInstance(),
            "DontDestroyOnLoad debe mantener la misma instancia tras cambiar de escena");
        Assert.AreEqual(2, GestionMonedas.GetInstance().GetMonedasAcumuladasInt());
    }

    // ---- Test 6: Awake sin SaveManager → NRE (documenta el bug) --------------
    /// <summary>
    /// Verifica que al instanciar GestionMonedas sin un SaveManager presente, se lance una NullReferenceException. Esto documenta un comportamiento esperado del sistema cuando se intenta acceder a datos de guardado sin que el SaveManager esté inicializado, lo que ayuda a identificar y corregir posibles errores en la gestión de dependencias entre componentes del juego.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator Awake_SinSaveManager_LanzaNullReference()
    {
        // Eliminar el SaveManager creado en SetUp
        Object.DestroyImmediate(saveManagerGO);
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");

        LogAssert.Expect(LogType.Exception, new Regex("NullReferenceException"));

        var go = new GameObject("GestionMonedas");
        go.AddComponent<GestionMonedas>(); // NRE en SaveManager.GetIntance().currentSave
        yield return null;

        // Bug latente: 'instance = this' se asignó ANTES del NRE, pero DontDestroyOnLoad
        // y la inicialización de monedas no se ejecutaron → singleton en estado inconsistente
        Assert.IsNotNull(GestionMonedas.GetInstance());
        Assert.AreEqual(0, GestionMonedas.GetInstance().GetMonedasAcumuladasInt());
    }
}