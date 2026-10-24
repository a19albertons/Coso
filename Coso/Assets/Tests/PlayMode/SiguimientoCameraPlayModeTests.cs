using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Pruebas de modo Play para la clase SiguimientoCamera.
/// Verifica el comportamiento de seguimiento de cámara a un objeto objetivo, incluyendo la interpolación, los límites y la funcionalidad de MuerteJugador.
/// </summary>
public class SiguimientoCameraPlayModeTests
{
    private const string testSceneName = "Tests";
    private GameObject cameraGO;
    private GameObject targetGO;

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
    /// Se ejecuta después de cada prueba en modo PlayMode. Destruye los objetos persistentes y resetea los campos estáticos.
    /// </summary>
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        SceneManager.LoadScene(testSceneName, LoadSceneMode.Single);
        yield return null;
    }

    // ---- Test 1: SiguimientoCamera sigue al objetivo con interpolación lineal ---
    /// <summary>
    /// Verifica que la cámara se mueva hacia la posición deseada utilizando Lerp.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoCamera_SigueObjetivoConLerp()
    {
        // Crear la cámara
        cameraGO = new GameObject("SiguimientoCamera");
        cameraGO.AddComponent<SiguimientoCamera>();
        var sc = cameraGO.GetComponent<SiguimientoCamera>();

        // Crear el objetivo
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        // Asignar el objetivo a la cámara
        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        // Posición inicial de la cámara
        var startPosition = new Vector3(5, 0, 0);
        cameraGO.transform.position = startPosition;

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }
        // Mover el objetivo a la derecha
        targetTransform.position = new Vector3(10, 0, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }
        // Verificar que la cámara se haya movido hacia el objetivo (no necesariamente igual debido a la interpolación)
        var finalPosition = cameraGO.transform.position;
        Assert.Greater(finalPosition.x, 5f, "La cámara debe haberse movido hacia la derecha");
        Assert.LessOrEqual(finalPosition.x, 10f, "La cámara no puede estar exactamente en el objetivo en una sola frame");
    }

    // ---- Test 2: SiguimientoCamera usa el offset correcto ---
    /// <summary>
    /// Verifica que la cámara se posicione correctamente respecto al objetivo utilizando el offset definido.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoCamera_UsaOffsetCorrecto()
    {
        // Crear la cámara con offset personalizado
        cameraGO = new GameObject("SiguimientoCamera");
        var sc = cameraGO.AddComponent<SiguimientoCamera>();

        // Asignar un offset específico
        ReflectionHelper.GetField(typeof(SiguimientoCamera), "offset").SetValue(sc, new Vector3(2f, 3f, -10f));

        // Crear el objetivo
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        // Asignar el objetivo a la cámara
        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }
        // Mover el objetivo a la derecha
        targetTransform.position = new Vector3(10, 0, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }
        // Verificar que la cámara esté en la posición deseada + offset
        var finalPosition = cameraGO.transform.position;
        Assert.AreEqual(finalPosition.x, 12f, 0.1f, "La cámara debería estar en x = 10 + 2");
        Assert.AreEqual(finalPosition.y, 3f, 0.1f, "La cámara debería estar en y = 0 + 3");
        Assert.AreEqual(finalPosition.z, -10f, 0.1f, "La cámara debería estar en z = 0 + (-10)");
    }

    // ---- Test 3: SiguimientoCamera respetar límites en eje Y ---
    /// <summary>
    /// Verifica que la cámara no baje del suelo (y >= 0).
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoCamera_RespetaNivelMinimoY()
    {
        // Crear la cámara con offset negativo en Y
        cameraGO = new GameObject("SiguimientoCamera");
        var sc = cameraGO.AddComponent<SiguimientoCamera>();

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "offset").SetValue(sc, new Vector3(0, -5f, -10f));

        // Crear el objetivo
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Mover el objetivo a una posición baja
        targetTransform.position = new Vector3(10, -5, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Verificar que la cámara no baje del suelo (y >= 0)
        var finalPosition = cameraGO.transform.position;
        Assert.GreaterOrEqual(finalPosition.y, 0f, "La cámara no debe bajar del suelo");
        Assert.AreEqual(0f, finalPosition.y, 0.1f, "La cámara debería estar en y = 0 cuando el objetivo está en y = -5 con offset = -5");
    }

    // ---- Test 4: SiguimientoCamera respetar límites en eje Y-Z ---
    /// <summary>
    /// Verifica que la cámara no suba demasiado (y - z <= 13).
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoCamera_RespetaNivelMaximoY()
    {
        // Crear la cámara con offset positivo en Y
        cameraGO = new GameObject("SiguimientoCamera");
        var sc = cameraGO.AddComponent<SiguimientoCamera>();

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "offset").SetValue(sc, new Vector3(0, 5f, -10f));

        // Crear el objetivo
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 15, 0);

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        // Actualizar tiempo para permitir que la cámara se reposicione
        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null;
        }

        // Mover el objetivo a una posición alta
        targetTransform.position = new Vector3(10, 20, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Verificar que la cámara no suba demasiado (y - z <= 13)
        var finalPosition = cameraGO.transform.position;
        Assert.LessOrEqual(finalPosition.y, 13f, "La cámara no debe subir demasiado");
        Assert.AreEqual(3f, finalPosition.y, 0.1f, "La cámara debería estar en y = 3 cuando el objetivo está en y = 20 con offset = (0,5,-10)");
    }

    // ---- Test 5: SiguimientoCamera con suavizado de velocidad ---
    /// <summary>
    /// Verifica que el factor de suavizado afecta la velocidad de seguimiento.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoCamera_SuavizadoControlado()
    {
        // Crear la cámara con un factor de suavizado bajo (lento)
        cameraGO = new GameObject("SiguimientoCamera");
        var sc = cameraGO.AddComponent<SiguimientoCamera>();

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "offset").SetValue(sc, new Vector3(0, 0, 0));
        ReflectionHelper.GetField(typeof(SiguimientoCamera), "suavizado").SetValue(sc, 1f); // Muy lento

        // Crear el objetivo
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        // Posición inicial de la cámara
        cameraGO.transform.position = new Vector3(10, 0, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Mover el objetivo a la derecha
        targetTransform.position = new Vector3(20, 0, 0);

        // Permitir varias actualizaciones con tiempo normal
        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null;
        }

        var finalPosition = cameraGO.transform.position;

        // Con un factor de 1f, la cámara debería haberse movido poco pero significativamente
        Assert.GreaterOrEqual(finalPosition.x, 10f, "Con suavizado bajo, la cámara se mueve lentamente, pero en el eje X tiene que ser superior a 10");
        Assert.LessOrEqual(finalPosition.x, 15.5f, "Con suavizado bajo, la cámara, pero no debería ser superior a 15.5");
    }

    // ---- Test 6: MuerteJugador repositiona la cámara instantáneamente ---
    /// <summary>
    /// Verifica que el método MuerteJugador repositiona la cámara sin interpolación.
    /// </summary>
    [UnityTest]
    public IEnumerator MuerteJugador_ReposicionaInstantaneamente()
    {
        // Crear la cámara con offset personalizado
        cameraGO = new GameObject("SiguimientoCamera");
        var sc = cameraGO.AddComponent<SiguimientoCamera>();

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "offset").SetValue(sc, new Vector3(2f, 5f, -10f));

        // Crear el objetivo
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        // Posición inicial de la cámara
        cameraGO.transform.position = new Vector3(5, 0, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }


        // Llamar a MuerteJugador
        sc.MuerteJugador();

        // Verificar que la cámara esté en la posición deseada
        var finalPosition = cameraGO.transform.position;
        Assert.AreEqual(finalPosition.x, 2f, 0.1f, "La cámara debería estar en x = 0 + 2");
        // Nunca se va a exceder que y sea mayor a 3.
        Assert.AreEqual(finalPosition.y, 3f, 0.1f, "La cámara debería estar en y = 0 + 3");
        Assert.AreEqual(finalPosition.z, -10f, 0.1f, "La cámara debería estar en z = 0 + (-10)");
    }

    // ---- Test 7: SiguimientoCamera inicializa correctamente ---
    /// <summary>
    /// Verifica que al crear una instancia de SiguimientoCamera, se inicie correctamente sin errores.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoCamera_Inicializacion_Correcta()
    {
        cameraGO = new GameObject("SiguimientoCamera");
        var sc = cameraGO.AddComponent<SiguimientoCamera>();

        // Crear el objetivo es obligatorio no se contempla su ausencia
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Verificar que la instancia se creó correctamente
        Assert.IsNotNull(sc);
        Assert.AreEqual(new Vector3(0f, 5f, -10f), ReflectionHelper.GetField(typeof(SiguimientoCamera), "offset").GetValue(sc), "El offset debe ser el valor por defecto");
        Assert.AreEqual(5f, ReflectionHelper.GetField(typeof(SiguimientoCamera), "suavizado").GetValue(sc), "El suavizado debe ser el valor por defecto");
    }

    // ---- Test 8: SiguimientoCamera con target en movimiento ---
    /// <summary>
    /// Verifica que la cámara siga correctamente a un objetivo que se mueve en múltiples frames.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoCamera_TargetEnMovimiento()
    {
        // Crear la cámara con offset
        cameraGO = new GameObject("SiguimientoCamera");
        var sc = cameraGO.AddComponent<SiguimientoCamera>();

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "offset").SetValue(sc, new Vector3(0, 5f, -10f));

        // Crear el objetivo
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 0, 0);

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        // Posición inicial de la cámara
        cameraGO.transform.position = new Vector3(5, 0, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Mover el objetivo en eje X
        targetTransform.position = new Vector3(10, 0, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Mover el objetivo en eje Y
        targetTransform.position = new Vector3(10, 10, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        var finalPosition = cameraGO.transform.position;

        // Verificar que la cámara siga al objetivo con el offset
        Assert.AreEqual(finalPosition.x, 10f, 0.1f, "La cámara debería seguir el movimiento en X");
        Assert.AreEqual(finalPosition.y, 3f, 0.1f, "La cámara no debería exceder el límite en Y de 3");
        Assert.AreEqual(finalPosition.z, -10f, 0.1f, "La cámara debería mantener Z con offset");
    }

    // ---- Test 9: SiguimientoCamera no se sale de límites cuando el objetivo está muy alto ---
    /// <summary>
    /// Verifica que la cámara no suba más allá del límite superior cuando el objetivo está en una posición extremadamente alta.
    /// </summary>
    [UnityTest]
    public IEnumerator SiguimientoCamera_LimitSuperiorAlto()
    {
        // Crear la cámara con offset positivo en Y
        cameraGO = new GameObject("SiguimientoCamera");
        var sc = cameraGO.AddComponent<SiguimientoCamera>();

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "offset").SetValue(sc, new Vector3(0, 10f, -10f));

        // Crear el objetivo
        targetGO = new GameObject("Target");
        var targetTransform = targetGO.transform;
        targetTransform.position = new Vector3(0, 50, 0);

        ReflectionHelper.GetField(typeof(SiguimientoCamera), "target").SetValue(sc, targetTransform);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Mover el objetivo a una posición muy alta
        targetTransform.position = new Vector3(10, 100, 0);

        for (int i = 0; i < 60; i++)
        {
            Time.timeScale = 1f;
            yield return null; // Permitir actualización
        }

        // Verificar que la cámara no suba demasiado (y - z <= 13)
        var finalPosition = cameraGO.transform.position;
        Assert.LessOrEqual(finalPosition.y, 13f, "La cámara no debe subir demasiado");
        Assert.AreEqual(3f, finalPosition.y, 0.1f, "La cámara debería estar en y = 3 cuando el objetivo está en y = 100 con offset = (0,10,-10)");
    }
}
