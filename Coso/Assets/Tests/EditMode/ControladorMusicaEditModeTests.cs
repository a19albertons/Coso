using NUnit.Framework;
using UnityEngine;
using System.Reflection;

/// <summary>
/// Clase de pruebas unitarias para la clase ControladorMusica en modo EditMode. Estas pruebas verifican el comportamiento de la gestión de música sin necesidad de ejecutar el juego.
/// </summary>
public class ControladorMusicaEditModeTests
{
    private ControladorMusica gestor;
    private GameObject go;
    private FieldInfo instanceField;
    private FieldInfo audioSourceField;

    /// <summary>
    /// Crea un nuevo GameObject y añade el componente ControladorMusica antes de cada test. También obtiene referencias a los campos privados necesarios para las pruebas.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        go = new GameObject("TestControladorMusica");
        gestor = go.AddComponent<ControladorMusica>(); // Awake NO se llama en EditMode

        instanceField = typeof(ControladorMusica)
            .GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
        audioSourceField = typeof(ControladorMusica)
            .GetField("audioSource", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    /// <summary>
    /// Limpia el singleton y destruye el GameObject después de cada test para evitar efectos colaterales entre tests.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
        instanceField.SetValue(null, null); // limpiar singleton estático entre tests
    }

    /// <summary>
    /// Verifica que el estado inicial del singleton sea null antes de crear una instancia.
    /// </summary>
    [Test]
    public void GetInstance_SinInstanciaActiva_DevuelveNull()
    {
        Assert.IsNull(ControladorMusica.GetInstance());
    }

    /// <summary>
    /// Verifica que el método GetInstance devuelva null antes de crear el objeto.
    /// </summary>
    [Test]
    public void GetInstance_AntesDeCrear_DevuelveNull()
    {
        Assert.IsNull(ControladorMusica.GetInstance());
    }
}
