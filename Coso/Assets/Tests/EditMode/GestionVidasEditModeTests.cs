using NUnit.Framework;
using UnityEngine;
using System.Reflection;

/// <summary>
/// Clase de pruebas unitarias para la clase GestionVidas en modo EditMode. Estas pruebas verifican el comportamiento de la gestión de vidas sin necesidad de ejecutar el juego.
/// </summary>
public class GestionVidasEditModeTests
{
    private GestionVidas gestor;
    private GameObject go;
    private FieldInfo vidasField;
    private FieldInfo vidasActualesField;
    private FieldInfo instanceField;

    /// <summary>
    /// Crea un nuevo GameObject y añade el componente GestionVidas antes de cada test. También obtiene referencias a los campos privados necesarios para las pruebas.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        go = new GameObject("TestGestionVidas");
        gestor = go.AddComponent<GestionVidas>(); // Awake NO se llama en EditMode

        vidasField = ReflectionHelper.GetField(typeof(GestionVidas), "vidas");
        vidasActualesField = ReflectionHelper.GetField(typeof(GestionVidas), "vidasActuales");
        instanceField = ReflectionHelper.GetField(typeof(GestionVidas), "instance");
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
    /// Verifica que el estado inicial de vidas sea 5 (valor por defecto).
    /// </summary>
    [Test]
    public void EstadoInicial_EsCinco()
    {
        Assert.AreEqual(5, vidasField.GetValue(gestor));
        Assert.AreEqual(0, vidasActualesField.GetValue(gestor)); // Este no se le asigna valor inicial, pero en C# es 0 no nulo
    }

    /// <summary>
    /// Verifica que el método GetInstance devuelva null antes de crear una instancia.
    /// </summary>
    [Test]
    public void GetInstance_SinInstanciaActiva_DevuelveNull()
    {
        Assert.IsNull(GestionVidas.GetInstance());
    }

    /// <summary>
    /// Verifica que el método GetInstance devuelva null antes de crear el objeto.
    /// </summary>
    [Test]
    public void GetInstance_AntesDeCrear_DevuelveNull()
    {
        Assert.IsNull(GestionVidas.GetInstance());
    }

    /// <summary>
    /// Verifica que PlayerPierdeVida reduce las vidas en uno.
    /// </summary>
    [Test]
    public void PlayerPierdeVida_ReduceVidasEnUno()
    {
        vidasActualesField.SetValue(gestor, 5);
        gestor.PlayerPierdeVida();
        Assert.AreEqual(4, vidasActualesField.GetValue(gestor));
    }

    /// <summary>
    /// Verifica que PlayerPierdeVida no reduce las vidas cuando ya son 0.
    /// </summary>
    [Test]
    public void PlayerPierdeVida_CuandoVidasCero_NoReduce()
    {
        vidasActualesField.SetValue(gestor, 0);
        gestor.PlayerPierdeVida();
        Assert.AreEqual(0, vidasActualesField.GetValue(gestor));
    }

    /// <summary>
    /// Verifica que PlayerObtieneVida aumenta las vidas en uno.
    /// </summary>
    [Test]
    public void PlayerObtieneVida_AumentaVidasEnUno()
    {
        vidasActualesField.SetValue(gestor, 3);
        gestor.PlayerObtieneVida();
        Assert.AreEqual(4, vidasActualesField.GetValue(gestor));
    }

    /// <summary>
    /// Verifica que ResetVidas restaura las vidas al valor inicial.
    /// </summary>
    [Test]
    public void ResetVidas_RestauraVidasAlValorInicial()
    {
        vidasActualesField.SetValue(gestor, 2);
        gestor.ResetVidas();
        Assert.AreEqual(5, vidasActualesField.GetValue(gestor));
    }

    /// <summary>
    /// Verifica que GetVidasActuales devuelve la cantidad de vidas como string.
    /// </summary>
    [Test]
    public void GetVidasActuales_DevuelveStringCorrecto()
    {
        vidasActualesField.SetValue(gestor, 3);
        Assert.AreEqual("3", gestor.GetVidasActuales());
    }

    /// <summary>
    /// Verifica que GetVidasActualesInt devuelve la cantidad de vidas como entero.
    /// </summary>
    [Test]
    public void GetVidasActualesInt_DevuelveEnteroCorrecto()
    {
        vidasActualesField.SetValue(gestor, 5);
        Assert.AreEqual(5, gestor.GetVidasActualesInt());
    }
}
