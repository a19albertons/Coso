using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
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
        var botonLoadGameGO = new GameObject("LoadGAmeButton");
        var botonSalirGO = new GameObject("SalirDelJuego");
        var mainMenuGO = new GameObject("MainMenu");
        var slotsGO = new GameObject("Slots");
        var botonNuevoJuego = botonNuevoJuegoGO.AddComponent<Button>(); // Añadir un botón para evitar errores de referencia nula
        var botonLoadGame = botonLoadGameGO.AddComponent<Button>(); // Añadir un botón para evitar errores de referencia nula
        var botonSalir = botonSalirGO.AddComponent<Button>();
        slotsGO.SetActive(false); // Inicialmente inactivo
        ReflectionHelper.GetField(typeof(StartGame), "newGameButton").SetValue(startGameGO.GetComponent<StartGame>(), botonNuevoJuego);
        ReflectionHelper.GetField(typeof(StartGame), "loadGameButton").SetValue(startGameGO.GetComponent<StartGame>(), botonLoadGame);
        ReflectionHelper.GetField(typeof(StartGame), "exitGameButton").SetValue(startGameGO.GetComponent<StartGame>(), botonSalir);
        ReflectionHelper.GetField(typeof(StartGame), "mainMenu").SetValue(startGameGO.GetComponent<StartGame>(), mainMenuGO);
        ReflectionHelper.GetField(typeof(StartGame), "slots").SetValue(startGameGO.GetComponent<StartGame>(), slotsGO);

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

    // ---- Test 1: Start asigna evento al botón New Game ------------------------
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

    // ---- Test 2: Start asigna evento al botón Load Game ---------
    [UnityTest]
    public IEnumerator Start_AsignaEventoAlBotonLoadGame()
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
        FieldInfo loadGameButton = ReflectionHelper.GetField(typeof(StartGame), "loadGameButton");
        Assert.IsNotNull(loadGameButton, "loadGameButton debe existir");
        Button continueButtonConvertido = (Button)loadGameButton.GetValue(startGame);
        Assert.IsTrue(continueButtonConvertido.onClick != null, "onClick de loadGameButton debe no ser null");
    }

    // ---- Test 3: Start selecciona newGameButton como activo por defecto ---------
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

    // ---- Test 4: NewGame() cambia la visibilidad de mainMenu y slots ---------
    [UnityTest]
    public IEnumerator Start_NewGameCambiamVisibilidadDeMainMenuYSlots()
    {
        var startGame = startGameGO.GetComponent<StartGame>();
        yield return null;
        
        // Verificar estado inicial (newGame = false)
        bool initialState = StartGame.newGame;
        Assert.IsFalse(initialState, "newGame debe ser false al inicio");

        yield return null;

        // Obtener referencias a los GameObjects
        FieldInfo mainMenuField = ReflectionHelper.GetField(typeof(StartGame), "mainMenu");
        FieldInfo slotsField = ReflectionHelper.GetField(typeof(StartGame), "slots");
        Assert.IsNotNull(mainMenuField, "mainMenu debe existir");
        Assert.IsNotNull(slotsField, "slots debe existir");

        GameObject mainMenu = (GameObject)mainMenuField.GetValue(startGame);
        GameObject slots = (GameObject)slotsField.GetValue(startGame);

        // Verificar estado inicial de visibilidad
        Assert.IsTrue(mainMenu.activeSelf, "mainMenu debe ser activo al inicio");
        Assert.IsFalse(slots.activeSelf, "slots debe ser inactivo al inicio");

        yield return null;

        // Ejecutar el método NewGame() mediante reflexión
        MethodInfo newGameMethod = ReflectionHelper.GetMethodInfo(typeof(StartGame),"NewGame");
        newGameMethod.Invoke(startGame, null);

        yield return null;

        // Verificar estado después de llamar a NewGame()
        Assert.IsTrue(StartGame.newGame, "newGame debe ser true después de NewGame()");
        Assert.IsFalse(mainMenu.activeSelf, "mainMenu debe ser inactivo después de NewGame()");
        Assert.IsTrue(slots.activeSelf, "slots debe ser activo después de NewGame()");
    }

    // ---- Test 5: LoadGame() cambia la visibilidad de mainMenu y slots ---------
    [UnityTest]
    public IEnumerator Start_LoadGameCambiamVisibilidadDeMainMenuYSlots()
    {
        var startGame = startGameGO.GetComponent<StartGame>();
        yield return null;

        // Verificar estado inicial (newGame = false)
        bool initialState = StartGame.newGame;
        Assert.IsFalse(initialState, "newGame debe ser false al inicio");

        yield return null;

        // Obtener referencias a los GameObjects
        FieldInfo mainMenuField = ReflectionHelper.GetField(typeof(StartGame), "mainMenu");
        FieldInfo slotsField = ReflectionHelper.GetField(typeof(StartGame), "slots");
        Assert.IsNotNull(mainMenuField, "mainMenu debe existir");
        Assert.IsNotNull(slotsField, "slots debe existir");

        GameObject mainMenu = (GameObject)mainMenuField.GetValue(startGame);
        GameObject slots = (GameObject)slotsField.GetValue(startGame);

        // Verificar estado inicial de visibilidad
        Assert.IsTrue(mainMenu.activeSelf, "mainMenu debe ser activo al inicio");
        Assert.IsFalse(slots.activeSelf, "slots debe ser inactivo al inicio");

        yield return null;

        // Ejecutar el método LoadGame() mediante reflexión
        MethodInfo loadGameMethod = ReflectionHelper.GetMethodInfo(typeof(StartGame), "LoadGame");
        loadGameMethod.Invoke(startGame, null);

        yield return null;

        // Verificar estado después de llamar a LoadGame()
        Assert.IsFalse(StartGame.newGame, "newGame debe ser false después de LoadGame()");
        Assert.IsFalse(mainMenu.activeSelf, "mainMenu debe ser inactivo después de LoadGame()");
        Assert.IsTrue(slots.activeSelf, "slots debe ser activo después de LoadGame()");
    }
}
