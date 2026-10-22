using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Clase de pruebas unitarias para la clase CurrentSave, que representa la partida actual del juego.
/// </summary>
public class CurrentSaveEditModeTests
{
    [Test]
    public void CurrentSave_InicializadoConCamposPorDefecto()
    {
        var currentSave = new CurrentSave();
        Assert.IsNotNull(currentSave.playerInfo);
        Assert.AreEqual(0, currentSave.levels.Count);
        Assert.IsNotNull(currentSave.checkpoint);
    }

    [Test]
    public void CurrentSave_PlayerInfo_EsInicializado()
    {
        var currentSave = new CurrentSave();
        Assert.AreEqual(5, currentSave.playerInfo.vidasActuales);
        Assert.AreEqual(0, currentSave.playerInfo.monedasActuales);
        Assert.AreEqual(PowerUPType.None, currentSave.playerInfo.powerUpActual);
    }

    [Test]
    public void CurrentSave_Levels_AñadeNivel()
    {
        var currentSave = new CurrentSave();
        currentSave.levels.Add(new Level { identificadorNivel = "1-1" });
        Assert.AreEqual(1, currentSave.levels.Count);
    }

    [Test]
    public void CurrentSave_Checkpoint_EsInicializado()
    {
        var currentSave = new CurrentSave();
        Assert.IsFalse(currentSave.checkpoint.IsTutorialDone);
        Assert.IsFalse(currentSave.checkpoint.level1_4IsDone);
        Assert.IsFalse(currentSave.checkpoint.level1_8IsDone);
        Assert.IsFalse(currentSave.checkpoint.haMuerto);
        Assert.IsFalse(currentSave.checkpoint.JuegoCompletado);
    }

    [Test]
    public void CurrentSave_EsSerializable()
    {
        var currentSave = new CurrentSave
        {
            playerInfo = new PlayerInfo { vidasActuales = 3, monedasActuales = 10, powerUpActual = PowerUPType.DoubleJump },
            levels = new List<Level> { new Level { identificadorNivel = "1-2" } },
            checkpoint = new Checkpoint { IsTutorialDone = true }
        };
        string json = JsonUtility.ToJson(currentSave);
        var restored = JsonUtility.FromJson<CurrentSave>(json);
        Assert.AreEqual(3, restored.playerInfo.vidasActuales);
        Assert.AreEqual(10, restored.playerInfo.monedasActuales);
        Assert.AreEqual(PowerUPType.DoubleJump, restored.playerInfo.powerUpActual);
        Assert.AreEqual(1, restored.levels.Count);
        Assert.AreEqual("1-2", restored.levels[0].identificadorNivel);
        Assert.IsTrue(restored.checkpoint.IsTutorialDone);
    }
}
