using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Pruebas de modo Play para la clase AlternarBackground.
/// Verifica el comportamiento de alternar el sprite según la relación de aspecto de la pantalla.
/// </summary>
public class AlternarBackgroundPlayModeTests
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
        yield return null;
    }

    // ---- Test 1: AlternarBackground alterna sprite cuando aspect ratio > threshold ---
    /// <summary>
    /// Verifica que cuando la relación de aspecto es mayor al umbral, se cambia el sprite a uno alternativo.
    /// </summary>
    [UnityPlatform(RuntimePlatform.WindowsPlayer, RuntimePlatform.OSXPlayer, RuntimePlatform.LinuxPlayer, RuntimePlatform.WebGLPlayer, RuntimePlatform.Android, RuntimePlatform.IPhonePlayer)]
    [UnityTest]
    public IEnumerator AlternarBackground_AlternaSprite_CuandoAspectRatioMayorQueThreshold()
    {
        var go = new GameObject("AlternarBackgroundTest");
        go.AddComponent<SpriteRenderer>();
        var ab = go.AddComponent<AlternarBackground>();

        // Simular pantalla con aspect ratio > 1.78 (ej: 16:9 = 1.77, 16:10 = 1.6, pero usaremos 2.0)
        var originalWidth = Screen.width;
        var originalHeight = Screen.height;

        // Simular un aspect ratio alto (ej: 2.0)
        Screen.SetResolution(2000, 1000, true); // 2.0 aspect ratio
        Assert.IsNull(ab.GetComponent<SpriteRenderer>().sprite, "El sprite inicial debería ser null antes de asignar uno alternativo.");

        // crear un sprite alternativo (simulado)
        Sprite alternativeSprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, Texture2D.blackTexture.width, Texture2D.blackTexture.height), new Vector2(0.5f, 0.5f));

        // Obtener variable privada spriteAlternativo mediante reflexión y asignar valor
        var spriteAlternativoField = ReflectionHelper.GetField(typeof(AlternarBackground), "spriteAlternativo");
        spriteAlternativoField.SetValue(ab, alternativeSprite);

        yield return null;

        // Llamar al método Awake para que se ejecute la lógica de alternancia
        var awakeMethod = ReflectionHelper.GetMethodInfo(typeof(AlternarBackground), "Awake");
        awakeMethod.Invoke(ab, null);

        // Obtenemos el componente SpriteRenderer para verificar si el sprite se ha cambiado
        var spriteRenderer = go.GetComponent<SpriteRenderer>();

        // Si se esta aplicando el cambio de sprite por el aspect ratio, el sprite del SpriteRenderer debería ser el sprite alternativo y no ser null
        Assert.AreEqual(alternativeSprite, spriteRenderer.sprite, "Debería estar usando el sprite que se creo previamente como sprite alternativo.");

        // Restaurar resolución original
        Screen.SetResolution(originalWidth, originalHeight, true);

        yield return null;
    }

    // ---- Test 2: AlternarBackground no alterna sprite cuando aspect ratio <= threshold ---
    /// <summary>
    /// Verifica que cuando la relación de aspecto es menor o igual al umbral, el sprite no se cambia.
    /// </summary>
    [UnityPlatform(RuntimePlatform.WindowsPlayer, RuntimePlatform.OSXPlayer, RuntimePlatform.LinuxPlayer, RuntimePlatform.Android, RuntimePlatform.IPhonePlayer, RuntimePlatform.WebGLPlayer)]
    [UnityTest]
    public IEnumerator AlternarBackground_NoAlternaSprite_CuandoAspectRatioMenorQueThreshold()
    {
        var go = new GameObject("AlternarBackgroundTest");
        go.AddComponent<SpriteRenderer>();
        var ab = go.AddComponent<AlternarBackground>();

        // Simular un aspect ratio normal (ej: 16:9 = 1.77)
        var originalWidth = Screen.width;
        var originalHeight = Screen.height;

        Screen.SetResolution(1920, 1080, true); // 1.778 aspect ratio

        // crear un sprite alternativo (simulado)
        Sprite alternativeSprite = Sprite.Create(Texture2D.blackTexture, new Rect(0, 0, Texture2D.blackTexture.width, Texture2D.blackTexture.height), new Vector2(0.5f, 0.5f));

        // Obtener variable privada spriteAlternativo mediante reflexión y asignar valor
        var spriteAlternativoField = ReflectionHelper.GetField(typeof(AlternarBackground), "spriteAlternativo");
        spriteAlternativoField.SetValue(ab, alternativeSprite);

        yield return null;

        Debug.Log($"Screen real: {Screen.width}x{Screen.height} => {(float)Screen.width / Screen.height}");
        Assert.AreEqual(1920, Screen.width);
        Assert.AreEqual(1080, Screen.height);

        // Llamar al método Awake para que se ejecute la lógica de alternancia
        var awakeMethod = ReflectionHelper.GetMethodInfo(typeof(AlternarBackground), "Awake");
        awakeMethod.Invoke(ab, null);

        // No se debería usar el sprite alternativo, por lo tanto el sprite debería ser nulo
        var spriteRenderer = go.GetComponent<SpriteRenderer>();
        Assert.AreNotEqual(alternativeSprite, spriteRenderer.sprite, "El sprite no debería cambiarse cuando el aspect ratio es menor o igual al umbral.");

        // Restaurar resolución original
        Screen.SetResolution(originalWidth, originalHeight, true);

        yield return null;
    }
}
