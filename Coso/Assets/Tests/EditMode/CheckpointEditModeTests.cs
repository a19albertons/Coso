using NUnit.Framework;
using UnityEngine;

/// <summary>
/// CheckpointEditModeTests es una clase de pruebas unitarias para la clase Checkpoint.
/// </summary>
public class CheckpointEditModeTests
{
    [Test]
    public void Checkpoint_InicializadoConTodosLosCamposEnFalse()
    {
        var checkpoint = new Checkpoint();
        Assert.IsFalse(checkpoint.IsTutorialDone);
        Assert.IsFalse(checkpoint.level1_4IsDone);
        Assert.IsFalse(checkpoint.level1_8IsDone);
        Assert.IsFalse(checkpoint.haMuerto);
        Assert.IsFalse(checkpoint.JuegoCompletado);
    }

    [Test]
    public void Checkpoint_IsTutorialDone_Cambia()
    {
        var checkpoint = new Checkpoint();
        checkpoint.IsTutorialDone = true;
        Assert.IsTrue(checkpoint.IsTutorialDone);
    }

    [Test]
    public void Checkpoint_Level1_4IsDone_Cambia()
    {
        var checkpoint = new Checkpoint();
        checkpoint.level1_4IsDone = true;
        Assert.IsTrue(checkpoint.level1_4IsDone);
    }

    [Test]
    public void Checkpoint_Level1_8IsDone_Cambia()
    {
        var checkpoint = new Checkpoint();
        checkpoint.level1_8IsDone = true;
        Assert.IsTrue(checkpoint.level1_8IsDone);
    }

    [Test]
    public void Checkpoint_HaMuerto_Cambia()
    {
        var checkpoint = new Checkpoint();
        checkpoint.haMuerto = true;
        Assert.IsTrue(checkpoint.haMuerto);
    }

    [Test]
    public void Checkpoint_JuegoCompletado_Cambia()
    {
        var checkpoint = new Checkpoint();
        checkpoint.JuegoCompletado = true;
        Assert.IsTrue(checkpoint.JuegoCompletado);
    }

    [Test]
    public void Checkpoint_EsSerializable()
    {
        var checkpoint = new Checkpoint
        {
            IsTutorialDone = true,
            level1_4IsDone = false,
            level1_8IsDone = true,
            haMuerto = false,
            JuegoCompletado = true
        };
        string json = JsonUtility.ToJson(checkpoint);
        var restored = JsonUtility.FromJson<Checkpoint>(json);
        Assert.IsTrue(restored.IsTutorialDone);
        Assert.IsFalse(restored.level1_4IsDone);
        Assert.IsTrue(restored.level1_8IsDone);
        Assert.IsFalse(restored.haMuerto);
        Assert.IsTrue(restored.JuegoCompletado);
    }
}
