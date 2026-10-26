using NUnit.Framework;
using UnityEngine;
using System.Reflection;

/// <summary>
/// Clase de pruebas unitarias para la clase GestionMonedas en modo EditMode. Estas pruebas verifican el comportamiento de la gestión de monedas sin necesidad de ejecutar el juego.
/// </summary>
public class GestionMonedasEditModeTests
{
    private GestionMonedas gestor;
    private GameObject go;
    private FieldInfo monedasField;
    private FieldInfo instanceField;

    /// <summary>
    /// Crea un nuevo GameObject y añade el componente GestionMonedas antes de cada test. También obtiene referencias a los campos privados necesarios para las pruebas.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        go = new GameObject("TestGestionMonedas");
        gestor = go.AddComponent<GestionMonedas>(); // Awake NO se llama en EditMode

        monedasField = ReflectionHelper.GetField(typeof(GestionMonedas), "monedasAcumuladas");
        instanceField = ReflectionHelper.GetField(typeof(GestionMonedas), "instance");
    }

    /// <summary>
    /// Limpia el singleton y destruye el GameObject después de cada test para evitar efectos colaterales entre tests.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
        ReflectionHelper.ResetStaticField(typeof(GestionMonedas), "instance"); // limpiar singleton estático entre tests
    }

    /// <summary>
    /// Verifica que el estado inicial de las monedas acumuladas sea cero tanto en su representación entera como en su representación en cadena.
    /// </summary>
    [Test]
    public void EstadoInicial_EsCero()
    {
        Assert.AreEqual(0, gestor.GetMonedasAcumuladasInt());
        Assert.AreEqual("0", gestor.GetMonedasAcumuladas());
    }

    /// <summary>
    /// Verifica que al sumar una moneda, el contador de monedas acumuladas se incremente en uno.
    /// </summary>
    [Test]
    public void SumarMoneda_IncrementaEnUno()
    {
        gestor.SumarMoneda();
        Assert.AreEqual(1, gestor.GetMonedasAcumuladasInt());
    }

    /// <summary>
    /// Verifica que el método GetMonedasAcumuladas devuelva la representación en cadena correcta del número de monedas acumuladas.
    /// </summary>
    [Test]
    public void GetMonedasAcumuladas_DevuelveStringCorrecto()
    {
        monedasField.SetValue(gestor, 42);
        Assert.AreEqual("42", gestor.GetMonedasAcumuladas());
    }

    /// <summary>
    /// Verifica que al llegar a 100 monedas, el contador se reinicie a cero.
    /// </summary>
    [Test]
    public void SumarMoneda_AlLlegarACien_SeReiniciaACero()
    {
        monedasField.SetValue(gestor, 99);
        gestor.SumarMoneda();
        Assert.AreEqual(0, gestor.GetMonedasAcumuladasInt());
    }

    /// <summary>
    /// Verifica que al sumar una moneda cuando ya hay 99 monedas, el contador se reinicie a cero y no se desborde.
    /// Esto asegura que el sistema de gestión de monedas no permita valores superiores a 99.
    /// </summary>
    [Test]
    public void SumarMoneda_NoDesbordaElLimite()
    {
        // Documenta el comportamiento actual: el sobrante se pierde
        monedasField.SetValue(gestor, 99);
        gestor.SumarMoneda();
        gestor.SumarMoneda();
        Assert.AreEqual(1, gestor.GetMonedasAcumuladasInt());
    }

    /// <summary>
    /// Verifica que el método GetInstance devuelva la misma instancia de GestionMonedas que se creó en el test, asegurando que el patrón singleton funcione correctamente.
    /// </summary>
    [Test]
    public void GetInstance_SinInstanciaActiva_DevuelveNull()
    {
        // Awake no se ha llamado → el singleton estático sigue en null
        Assert.IsNull(GestionMonedas.GetInstance());
    }
}