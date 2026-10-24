using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Clase de pruebas en modo PlayMode para la clase StartGame. Estas pruebas verifican la funcionalidad de la pantalla de inicio del juego, incluyendo la gestión de botones, validación de guardados y navegación entre escenas.
/// </summary>
public class StartGamePlayModeTests
{
    private const string testSceneName = "Tests";
    private GameObject startGameGO;
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
        if (File.Exists(Path.Combine(Application.persistentDataPath, "slotGuardado1")))
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "slotGuardado1"));
        }
        yield return null;
    }

    /// <summary>
    /// Se ejecuta antes de cada prueba en modo PlayMode. Crea un nuevo GameObject para el SaveManager y lo inicializa.
    /// </summary>
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Resetear la instancia antes de crear el StartGameGO para evitar efectos colaterales
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");
        saveManagerGO = new GameObject("SaveManager");
        saveManagerGO.AddComponent<SaveManager>();
        yield return null;

        // Crear el StartGameGO
        startGameGO = new GameObject("StartGame");
        startGameGO.AddComponent<StartGame>();

        // Asignar botones para que no sean null
        var botonNuevoJuegoGO = new GameObject("NewGameButton");
        var botonContinueGO = new GameObject("ContinueButton");
        var botonNuevoJuego = botonNuevoJuegoGO.AddComponent<Button>(); // Añadir un botón para evitar errores de referencia nula
        var botonContinue = botonContinueGO.AddComponent<Button>(); // Añadir un botón para evitar errores de referencia nula
        botonContinueGO.AddComponent<TextMeshProUGUI>(); // Añadir un TextMeshProUGUI para el botón Continue
        ReflectionHelper.GetField(typeof(StartGame), "newGameButton").SetValue(startGameGO.GetComponent<StartGame>(), botonNuevoJuego);
        ReflectionHelper.GetField(typeof(StartGame), "continueButton").SetValue(startGameGO.GetComponent<StartGame>(), botonContinue);

        // Añadir un EventSystem a la escena para que los botones funcionen correctamente
        if (EventSystem.current == null)
        {
            var eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<EventSystem>();
        }
        yield return null;
    }

    /// <summary>
    /// Se ejecuta después de cada prueba en modo PlayMode. Destruye los objetos persistentes y resetea los campos estáticos de los singletons.
    /// </summary>
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (startGameGO != null && startGameGO.activeInHierarchy)
            Object.DestroyImmediate(startGameGO);

        if (SaveManager.GetInstance() != null)
            Object.DestroyImmediate(SaveManager.GetInstance().gameObject);

        yield return null;

        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");

        if (File.Exists(Path.Combine(Application.persistentDataPath, "slotGuardado1")))
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "slotGuardado1"));
        }
        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;
    }

    // ---- Test 1: ValidarBotonContinue con archivo existente devuelve true ------
    [UnityTest]
    public IEnumerator ValidarBotonContinue_Con_Existente_DevuelveTrue()
    {
        var sm = SaveManager.GetInstance();
        sm.currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 3, monedasActuales = 10, powerUpActual = PowerUPType.DoubleJump }
        };
        yield return null;
        sm.SaveGame(1);
        yield return null;

        // Usar el botón para invocar ValidarBotonContinue
        var startGame = startGameGO.GetComponent<StartGame>();
        MethodInfo ValidarBotonContinue = ReflectionHelper.GetMethodInfo(typeof(StartGame), "ValidarBotonContinue");
        Assert.IsTrue((bool)ValidarBotonContinue.Invoke(startGame, null));
    }

    // ---- Test 2: ValidarBotonContinue sin archivo devuelve false ----------------
    [UnityTest]
    public IEnumerator ValidarBotonContinue_Sin_Existente_DevuelveFalse()
    {
        ReflectionHelper.ResetStaticField(typeof(SaveManager), "instance");
        yield return null;

        var startGame = startGameGO.GetComponent<StartGame>();
        MethodInfo ValidarBotonContinue = ReflectionHelper.GetMethodInfo(typeof(StartGame), "ValidarBotonContinue");
        Assert.IsFalse((bool)ValidarBotonContinue.Invoke(startGame, null));
    }

    // ---- Test 3: Start asigna evento al botón New Game ------------------------
    [UnityTest]
    public IEnumerator Start_AsignaEventoAlBotonNewGame()
    {
        var startGame = startGameGO.GetComponent<StartGame>();
        Assert.IsNotNull(startGame);
        yield return null;

        // Verificar que el evento se ha añadido al botón

        FieldInfo newGameButton = ReflectionHelper.GetField(typeof(StartGame), "newGameButton");
        Assert.IsNotNull(newGameButton, "newGameButton debe existir");
        Button newGameButtonConvertido = (Button)newGameButton.GetValue(startGame);
        Assert.IsTrue(newGameButtonConvertido.onClick != null, "onClick de newGameButton debe no ser null");

        yield return null;
    }

    // ---- Test 4: Start asigna evento al botón Continue si existe save ---------
    [UnityTest]
    public IEnumerator Start_AsignaEventoAlBotonContinue_ExistenteSave()
    {
        var sm = SaveManager.GetInstance();
        sm.currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 3, monedasActuales = 10, powerUpActual = PowerUPType.DoubleJump }
        };
        yield return null;
        sm.SaveGame(1);
        yield return null;

        var startGame = startGameGO.GetComponent<StartGame>();
        FieldInfo continueButton = ReflectionHelper.GetField(typeof(StartGame), "continueButton");
        Assert.IsNotNull(continueButton, "continueButton debe existir");
        Button continueButtonConvertido = (Button)continueButton.GetValue(startGame);
        Assert.IsTrue(continueButtonConvertido.onClick != null, "onClick de continueButton debe no ser null");
    }

    // ---- Test 5: Start deshabilita botón Continue si no existe save ------------
    [UnityTest]
    public IEnumerator Start_DeshabilitaBotonContinue_SinExistenteSave()
    {
        // Limpiar save anterior
        var sm = SaveManager.GetInstance();
        sm.currentSave = null;
        yield return null;

        var startGame = startGameGO.GetComponent<StartGame>();
        FieldInfo continueButton = ReflectionHelper.GetField(typeof(StartGame), "continueButton");
        Assert.IsNotNull(continueButton, "continueButton debe existir");
        Button continueButtonConvertido = (Button)continueButton.GetValue(startGame);
        Assert.IsFalse(continueButtonConvertido.interactable, "continueButton debe ser no interactable");

        // Verificar que el texto esté en color gris
        TextMeshProUGUI text = continueButtonConvertido.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text, "Debe existir un TextMeshProUGUI en el botón");
        Assert.AreEqual(Color.gray, text.color, "El texto debe estar en color gris");
    }

    // ---- Test 6: Start selecciona newGameButton como activo por defecto ---------
    [UnityTest]
    public IEnumerator Start_SeleccionaNewGameButtonComoActivoPorDefecto()
    {
        var startGame = startGameGO.GetComponent<StartGame>();
        FieldInfo newGameButton = ReflectionHelper.GetField(typeof(StartGame), "newGameButton");
        Assert.IsNotNull(newGameButton, "newGameButton debe existir");
        Button newGameButtonConvertido = (Button)newGameButton.GetValue(startGame);
        Assert.IsNotNull(newGameButtonConvertido, "newGameButton debe existir");

        yield return null;

        // Verificar que EventSystem.current ha sido seleccionado
        EventSystem eventSystem = EventSystem.current;
        Assert.IsNotNull(eventSystem, "EventSystem debe existir");
        // Verificar que el newGameButton es el objeto seleccionado
        var selectedGO = eventSystem.currentSelectedGameObject;
        Assert.AreEqual(newGameButtonConvertido.gameObject, selectedGO, "newGameButton debe estar seleccionado por defecto");
    }

    // ---- Test 7: NewGame llama a SaveManager.NewGame() --------------------------
    [UnityTest]
    public IEnumerator NewGame_LlamaASaveManagerNewGame()
    {
        var startGame = startGameGO.GetComponent<StartGame>();
        FieldInfo newGameButton = ReflectionHelper.GetField(typeof(StartGame), "newGameButton");
        Assert.IsNotNull(newGameButton, "newGameButton debe existir");
        Button newGameButtonConvertido = (Button)newGameButton.GetValue(startGame);
        Assert.IsNotNull(newGameButtonConvertido, "newGameButton debe existir");

        // Simular click en el botón New Game
        newGameButtonConvertido.onClick.Invoke();
        yield return null;

        // Verificar que se ha cargado la escena Intro
        Assert.IsTrue(SceneManager.GetActiveScene().name == "Intro", "Debe haberse cargado la escena Intro");

        // Restablecer la escena de prueba para las siguientes pruebas
        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;
    }

    // ---- Test 8: ContinueGame llama a SaveManager.LoadGame(1) ------------------
    [UnityTest]
    public IEnumerator ContinueGame_LlamaALoadGame1()
    {
        var sm = SaveManager.GetInstance();
        sm.currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 3, monedasActuales = 10, powerUpActual = PowerUPType.DoubleJump }
        };
        yield return null;
        sm.SaveGame(1);
        yield return null;

        var startGame = startGameGO.GetComponent<StartGame>();
        // Llamar al método Start nuevamente para que se aplique el Save existente y se habilite el botón Continue
        MethodInfo ActualizarStart = ReflectionHelper.GetMethodInfo(typeof(StartGame), "Start");
        ActualizarStart.Invoke(startGame, null);
        FieldInfo continueButton = ReflectionHelper.GetField(typeof(StartGame), "continueButton");
        Assert.IsNotNull(continueButton, "continueButton debe existir");
        Button continueButtonConvertido = (Button)continueButton.GetValue(startGame);
        Assert.IsNotNull(continueButtonConvertido, "continueButton debe existir");

        // Simular click en el botón Continue
        continueButtonConvertido.onClick.Invoke();
        yield return null;

        // Verificar que se ha cargado la escena MundoBase
        Assert.AreEqual("MundoBase", SceneManager.GetActiveScene().name, "Debe haberse cargado la escena MundoBase");

        // Restablecer la escena de prueba para las siguientes pruebas
        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;

    }
}
