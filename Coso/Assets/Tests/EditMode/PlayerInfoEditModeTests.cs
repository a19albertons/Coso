using NUnit.Framework;
using UnityEngine;

public class PlayerInfoEditModeTests
{
    [Test]
    public void VidasActuales_InicializadoACinco()
    {
        var playerInfo = new PlayerInfo();
        Assert.AreEqual(5, playerInfo.vidasActuales);
    }

    [Test]
    public void MonedasActuales_InicializadoACero()
    {
        var playerInfo = new PlayerInfo();
        Assert.AreEqual(0, playerInfo.monedasActuales);
    }

    [Test]
    public void PowerUpActual_InicializadoANone()
    {
        var playerInfo = new PlayerInfo();
        Assert.AreEqual(PowerUPType.None, playerInfo.powerUpActual);
    }

    [Test]
    public void VidasActuales_CambiaCuandoSeEstablece()
    {
        var playerInfo = new PlayerInfo();
        playerInfo.vidasActuales = 3;
        Assert.AreEqual(3, playerInfo.vidasActuales);
    }

    [Test]
    public void MonedasActuales_CambiaCuandoSeEstablece()
    {
        var playerInfo = new PlayerInfo();
        playerInfo.monedasActuales = 10;
        Assert.AreEqual(10, playerInfo.monedasActuales);
    }

    [Test]
    public void PowerUpActual_CambiaCuandoSeEstablece()
    {
        var playerInfo = new PlayerInfo();
        playerInfo.powerUpActual = PowerUPType.DoubleJump;
        Assert.AreEqual(PowerUPType.DoubleJump, playerInfo.powerUpActual);
    }

    [Test]
    public void PlayerInfo_EsSerializable()
    {
        var playerInfo = new PlayerInfo { vidasActuales = 3, monedasActuales = 10, powerUpActual = PowerUPType.DoubleJump };
        string json = JsonUtility.ToJson(playerInfo);
        var restored = JsonUtility.FromJson<PlayerInfo>(json);
        Assert.AreEqual(3, restored.vidasActuales);
        Assert.AreEqual(10, restored.monedasActuales);
        Assert.AreEqual(PowerUPType.DoubleJump, restored.powerUpActual);
    }
}
