using NUnit.Framework;
using UnityEngine;
using System.Reflection;

/// <summary>
/// Clase de pruebas unitarias para la clase GestionPowerUP en modo EditMode. Estas pruebas verifican el comportamiento de la gestión de power ups sin necesidad de ejecutar el juego.
/// </summary>
public class GestionPowerUPEditModeTests
{
    private GestionPowerUP gestor;
    private GameObject go;
    private FieldInfo estadoPowerUPField;
    private FieldInfo instanceField;

    /// <summary>
    /// Crea un nuevo GameObject y añade el componente GestionPowerUP antes de cada test. También obtiene referencias a los campos privados necesarios para las pruebas.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        go = new GameObject("TestGestionPowerUP");
        gestor = go.AddComponent<GestionPowerUP>(); // Awake NO se llama en EditMode

        estadoPowerUPField = ReflectionHelper.GetField(typeof(GestionPowerUP), "estadoPowerUP");
        instanceField = ReflectionHelper.GetField(typeof(GestionPowerUP), "instance");
    }

    /// <summary>
    /// Limpia el singleton y destruye el GameObject después de cada test para evitar efectos colaterales entre tests.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
        ReflectionHelper.ResetStaticField(typeof(GestionPowerUP), "instance"); // limpiar singleton estático entre tests
    }

    /// <summary>
    /// Verifica que el método GetInstance devuelva null antes de crear una instancia.
    /// </summary>
    [Test]
    public void GetInstance_SinInstanciaActiva_DevuelveNull()
    {
        Assert.IsNull(GestionPowerUP.GetInstance());
    }

    /// <summary>
    /// Verifica que el método GetInstance devuelva null antes de crear el objeto.
    /// </summary>
    [Test]
    public void GetInstance_AntesDeCrear_DevuelveNull()
    {
        Assert.IsNull(GestionPowerUP.GetInstance());
    }

    /// <summary>
    /// Verifica que PlayerPierdePowerUP cambie el estado a NoneState.
    /// </summary>
    [Test]
    public void PlayerPierdePowerUP_CambiaEstadoANoneState()
    {
        // Tipo al que es cambiado cuando ejecutas PlayerPierdePowerUP
        var noneStateType = typeof(NoneState);

        // Creamos una maquina de estados controlada para poder setearla en el campo privado estadoPowerUP
        PowerUPStateMachine maquinaEstadosControlada = new PowerUPStateMachine();
        maquinaEstadosControlada.Initialize(new DoubleJumpState(new PlayerController()));
        estadoPowerUPField.SetValue(gestor, maquinaEstadosControlada);

        // Ejecutamos el metodo que queremos testear
        gestor.PlayerPierdePowerUP();

        // Obtenemos el estado actual de la maquina de estados para verificar que haya cambiado a NoneState
        PowerUPStateMachine estadoActual = (PowerUPStateMachine)estadoPowerUPField.GetValue(gestor);
        // Verificamos que el estado haya cambiado a NoneState
        Assert.AreEqual(noneStateType, estadoActual.CurrentState.GetType());
    }

    /// <summary>
    /// Verifica que PlayerObtienePowerUP cambia el estado al tipo de power up especificado.
    /// </summary>
    [Test]
    public void PlayerObtienePowerUP_CambiaEstadoAlTipoEspecificado()
    {
        var doubleJumpStateType = typeof(DoubleJumpState);

        // Creamos una maquina de estados controlada para poder setearla en el campo privado estadoPowerUP
        PowerUPStateMachine maquinaEstadosControlada = new PowerUPStateMachine();
        maquinaEstadosControlada.Initialize(new NoneState(new PlayerController()));
        estadoPowerUPField.SetValue(gestor, maquinaEstadosControlada);

        // Se genera un power con un player controller para poder pasarle un parametro al metodo PlayerObtienePowerUP
        gestor.PlayerObtienePowerUP(new DoubleJumpState(new PlayerController()));

        // Extraemos la maquina de estados para verificar el estado actual
        PowerUPStateMachine estadoActual = (PowerUPStateMachine)estadoPowerUPField.GetValue(gestor);

        Assert.AreEqual(doubleJumpStateType, estadoActual.CurrentState.GetType());
    }

    /// <summary>
    /// Verifica que EstadoPowerUP devuelve el estado actual del power up.
    /// </summary>
    [Test]
    public void EstadoPowerUP_DevuelveEstadoActual()
    {
        var doubleJumpStateType = typeof(DoubleJumpState);

        // Creamos una maquina de estados controlada para poder setearla en el campo privado estadoPowerUP
        PowerUPStateMachine maquinaEstadosControlada = new PowerUPStateMachine();
        maquinaEstadosControlada.Initialize(new DoubleJumpState(new PlayerController()));

        // Establece la máquina de estados
        estadoPowerUPField.SetValue(gestor, maquinaEstadosControlada);

        var resultado = gestor.EstadoPowerUP();
        Assert.AreEqual(doubleJumpStateType, resultado.GetType());
    }

    /// <summary>
    /// Verifica que TipoPowerUP devuelve el tipo de power up como enum.
    /// </summary>
    [Test]
    public void TipoPowerUP_DevuelveEnumCorrecto()
    {
        // Creamos una maquina de estados controlada para poder setearla en el campo privado estadoPowerUP
        PowerUPStateMachine maquinaEstadosControlada = new PowerUPStateMachine();
        maquinaEstadosControlada.Initialize(new DoubleJumpState(new PlayerController()));

        // Asigna el valor de la máquina de estados al campo privado estadoPowerUP del gestor
        estadoPowerUPField.SetValue(gestor, maquinaEstadosControlada);

        var tipo = gestor.TipoPowerUP();
        Assert.AreEqual(PowerUPType.DoubleJump, tipo);
    }
}
