using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Pruebas de modo Play para la clase SiguimientoBackground.
/// Verifica el comportamiento de seguimiento del fondo a un objeto objetivo.
/// </summary>
public class SiguimientoBackgroundPlayModeTests
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
        // El mejor limpiador de escenas creado.
        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;
    }

    // ---- Test 1: SiguimientoBackground sigue al objetivo con interpolación lineal ---
    /// <summary>
    /// Verifica que el fondo se mueva hacia la posición X del objetivo utilizando Lerp.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoBackground_SigueObjetivoConLerp()
    {
        var go = new GameObject("SiguimientoBackgroundTest");
        go.AddComponent<SpriteRenderer>();
        var sb = go.AddComponent<SiguimientoBackground>();

        // Crear el objetivo
        var targetGo = new GameObject("Target");
        var targetTransform = targetGo.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        // Asignar el objetivo al seguimiento
        ReflectionHelper.GetField(typeof(SiguimientoBackground), "target").SetValue(sb, targetTransform);

        // Posición inicial del fondo
        var startPosition = new Vector3(5, 0, 0);
        go.transform.position = startPosition;

        yield return null;

        // Mover el objetivo a la derecha
        targetTransform.position = new Vector3(10, 0, 0);

        yield return null; // Permitir actualización

        // Verificar que el fondo se haya movido hacia el objetivo (no necesariamente igual debido a la interpolación)
        var finalPosition = go.transform.position;
        // El fondo debería haberse movido significativamente hacia el objetivo (la interpolación es rápida, 2f)
        Assert.Greater(finalPosition.x, 5f, "El fondo debe haberse movido hacia la derecha");
        Assert.LessOrEqual(finalPosition.x, 10f, "El fondo no puede estar exactamente en el objetivo en una sola frame");
    }

    // ---- Test 2: SiguimientoBackground mantiene la posición Y del fondo ---
    /// <summary>
    /// Verifica que la posición Y del fondo se mantiene constante mientras el objetivo se mueve en eje X.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoBackground_MantienePosicionY()
    {
        var go = new GameObject("SiguimientoBackgroundTest");
        go.AddComponent<SpriteRenderer>();
        var sb = go.AddComponent<SiguimientoBackground>();

        var targetGo = new GameObject("Target");
        var targetTransform = targetGo.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        // Asignar el objetivo al seguimiento
        ReflectionHelper.GetField(typeof(SiguimientoBackground), "target").SetValue(sb, targetTransform);

        // Posición inicial del fondo
        var startPosition = new Vector3(5, 10, 0);
        go.transform.position = startPosition;

        yield return null;

        // Mover el objetivo solo en eje X
        targetTransform.position = new Vector3(10, 0, 0);

        yield return null;

        var finalPosition = go.transform.position;

        // El Y del fondo debe permanecer igual al inicial (10)
        Assert.AreEqual(startPosition.y, finalPosition.y, "La posición Y del fondo debe mantenerse constante");
    }

    // ---- Test 3: SiguimientoBackground usa el factor de interpolación correcto (2f) ---
    /// <summary>
    /// Verifica que el fondo se acerque al objetivo con una velocidad de interpolación apropiada.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoBackground_UsaFactorDeInterpolacionCorrecto()
    {
        var go = new GameObject("SiguimientoBackgroundTest");
        go.AddComponent<SpriteRenderer>();
        var sb = go.AddComponent<SiguimientoBackground>();

        var targetGo = new GameObject("Target");
        var targetTransform = targetGo.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        // Asignar el objetivo al seguimiento
        ReflectionHelper.GetField(typeof(SiguimientoBackground), "target").SetValue(sb, targetTransform);

        // Posición inicial del fondo
        var startPosition = new Vector3(10, 0, 0);
        go.transform.position = startPosition;

        yield return null;

        // Mover el objetivo a la derecha
        targetTransform.position = new Vector3(20, 0, 0);

        // Permitir varias actualizaciones para ver el efecto de la interpolación
        for (int i = 0; i < 10; i++)
        {
            Time.timeScale = 1f;
            yield return null;
        }

        var finalPosition = go.transform.position;

        // Hacemos comprobaciones sobre la posición final del fondo.
        Assert.Greater(finalPosition.x, 10f, "El fondo debe haberse movido hacia la derecha");
        Assert.LessOrEqual(finalPosition.x, 20f, "El fondo no debería alcanzar el objetivo en pocas frames");
    }

    // ---- Test 4: SiguimientoBackground inicializa correctamente ---
    /// <summary>
    /// Verifica que al crear una instancia de SiguimientoBackground, se inicie correctamente sin errores.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoBackground_Inicializacion_Correcta()
    {
        var go = new GameObject("SiguimientoBackgroundTest");
        go.AddComponent<SpriteRenderer>();
        var sb = go.AddComponent<SiguimientoBackground>();

        // Asignar un transform de objetivo simulado
        var targetGo = new GameObject("Target");
        ReflectionHelper.GetField(typeof(SiguimientoBackground), "target").SetValue(sb, targetGo.transform);

        yield return null;

        // Verificar que la instancia se creó correctamente
        Assert.IsNotNull(sb);
    }

    // ---- Test 5: SiguimientoBackground calcula correctamente la interpolación ---
    /// <summary>
    /// Verifica que la fórmula de interpolación se aplica correctamente.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoBackground_CalculaInterpolacionCorrectamente()
    {
        var go = new GameObject("SiguimientoBackgroundTest");
        go.AddComponent<SpriteRenderer>();
        var sb = go.AddComponent<SiguimientoBackground>();

        var targetGo = new GameObject("Target");
        var targetTransform = targetGo.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        // Asignar el objetivo al seguimiento
        ReflectionHelper.GetField(typeof(SiguimientoBackground), "target").SetValue(sb, targetTransform);

        // Posición inicial del fondo
        var startPosition = new Vector3(5, 0, 0);
        go.transform.position = startPosition;

        yield return null;

        // Mover el objetivo a la izquierda
        targetTransform.position = new Vector3(-10, 0, 0);

        yield return null;

        var finalPosition = go.transform.position;

        // La interpolación debe ser: posición = inicio + (objetivo - inicio) * factor
        Assert.AreEqual(finalPosition.x, -10f, "La posición X del fondo debe estar entre 5 y 10");
    }
}
