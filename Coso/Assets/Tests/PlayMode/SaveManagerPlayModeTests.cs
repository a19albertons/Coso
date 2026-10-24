using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Clase de pruebas en modo PlayMode para la clase SaveManager. Estas pruebas verifican la funcionalidad de guardado y carga de partidas, así como el patrón Singleton implementado en SaveManager.
/// </summary>
public class SaveManagerPlayModeTests
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
        // Resetear la instancia antes de crear el SaveManagerGO para evitar efectos colaterales
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");
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
        if (SaveManager.GetIntance() != null)
            Object.Destroy(SaveManager.GetIntance().gameObject);

        yield return null;

        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");

        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;
    }

    // ---- Test 1: GetInstance sin SaveManager activa devuelve null ----------
    [UnityTest]
    public IEnumerator GetInstance_SinInstancia_Activa_DevuelveNull()
    {
        // Resetear antes de verificar para asegurar que no haya instancia previa
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");
        yield return null;
        Assert.IsNull(SaveManager.GetIntance());
    }

    // ---- Test 2: NewGame inicializa currentSave ---------------------------
    [UnityTest]
    public IEnumerator NewGame_InicializaCurrentSave()
    {
        // Después de SetUp, la instancia existe. Resetear currentSave para asegurar que se inicializa correctamente.
        var sm = SaveManager.GetIntance();
        sm.currentSave = null;
        yield return null;
        sm.NewGame();
        yield return null;
        Assert.IsNotNull(sm.currentSave);
    }

    // ---- Test 3: SaveGame escribe un archivo JSON -------------------------
    [UnityTest]
    public IEnumerator SaveGame_EscribeArchivoJson()
    {
        var sm = SaveManager.GetIntance();
        sm.currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 3, monedasActuales = 10, powerUpActual = PowerUPType.DoubleJump }
        };
        yield return null;
        sm.SaveGame(1);
        yield return null;
        string filePath = Path.Combine(Application.persistentDataPath, "slotGuardado1");
        Assert.IsTrue(File.Exists(filePath), "El archivo de guardado debe existir");
        string content = File.ReadAllText(filePath);
        var restored = JsonUtility.FromJson<CurrentSave>(content);
        Assert.AreEqual(3, restored.playerInfo.vidasActuales);
        Assert.AreEqual(10, restored.playerInfo.monedasActuales);
        Assert.AreEqual(PowerUPType.DoubleJump, restored.playerInfo.powerUpActual);
    }

    // ---- Test 4: LoadGame lee y deserializa desde archivo -----------------
    [UnityTest]
    public IEnumerator LoadGame_LeeYDeserializaDesdeArchivo()
    {
        var sm = SaveManager.GetIntance();
        sm.currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 5, monedasActuales = 0, powerUpActual = PowerUPType.None }
        };
        yield return null;
        sm.SaveGame(2);
        yield return null;
        // Resetear currentSave antes de cargar
        sm.currentSave = null;
        yield return null;
        sm.LoadGame(2);
        yield return null;
        Assert.IsNotNull(sm.currentSave);
        Assert.AreEqual(5, sm.currentSave.playerInfo.vidasActuales);
        Assert.AreEqual(0, sm.currentSave.playerInfo.monedasActuales);
        Assert.AreEqual(PowerUPType.None, sm.currentSave.playerInfo.powerUpActual);
    }

    // ---- Test 5: LoadGame con archivo no existente devuelve null ------------
    [UnityTest]
    public IEnumerator LoadGame_Arquivo_No_Existente_DevuelveNull()
    {
        var sm = SaveManager.GetIntance();
        sm.currentSave = null;
        yield return null;
        sm.LoadGame(99);
        yield return null;
        Assert.IsNull(sm.currentSave);
    }

    // ---- Test 6: Singleton pattern - segunda instancia se destruye ----------
    [UnityTest]
    public IEnumerator Singleton_SeSegundaInstanciaSeDestruye()
    {
        var sm1 = SaveManager.GetIntance();
        yield return null;
        var go2 = new GameObject("G2");
        go2.AddComponent<SaveManager>();
        yield return null;
        var sm2 = SaveManager.GetIntance();
        Assert.AreSame(sm1, sm2);
    }

    // ---- Test 7: GetInstance antes de crear el objeto devuelve null ---------
    [UnityTest]
    public IEnumerator GetInstance_AntesDeCrear_DevuelveNull()
    {
        // Resetear antes de verificar
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");
        yield return null;
        Assert.IsNull(SaveManager.GetIntance());
    }

    // ---- Test 8: SaveGame con slot inválido -------------------------------
    [UnityTest]
    public IEnumerator SaveGame_ConSlotInvalido_EscribeArchivo()
    {
        var sm = SaveManager.GetIntance();
        sm.currentSave = new CurrentSave();
        yield return null;
        sm.SaveGame(0);
        yield return null;
        string filePath = Path.Combine(Application.persistentDataPath, "slotGuardado0");
        Assert.IsTrue(File.Exists(filePath), "El archivo de guardado debe existir para el slot 0");
    }

    // ---- Test 9: NewGame sin SaveManager inicializado ---------------------
    [UnityTest]
    public IEnumerator NewGame_SinSaveManagerInicializado()
    {
        // Resetear para simular que no hay instancia previa
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");
        yield return null;
        var go = new GameObject("TestNewGame");
        go.AddComponent<SaveManager>();
        yield return null;
        // Awake se llama, así que GetInstance debería ser no null ahora
        Assert.IsNotNull(SaveManager.GetIntance());
    }
}
