using NUnit.Framework;
using UnityEngine;

/// <summary>
/// LevelEditModeTests es una clase de pruebas unitarias para la clase Level.
/// </summary>
public class LevelEditModeTests
{
    [Test]
    public void Nivel_InicializadoConCamposPorDefecto()
    {
        var level = new Level();
        Assert.IsNull(level.identificadorNivel);
        Assert.IsNull(level.starsObtained);
        Assert.AreEqual(EnumStateLevelYStars.LevelYStars.NoObtenida, level.isLevelCompleted);
    }

    [Test]
    public void Nivel_IdentificadorNivel_Cambia()
    {
        var level = new Level();
        level.identificadorNivel = "1-1";
        Assert.AreEqual("1-1", level.identificadorNivel);
    }

    [Test]
    public void Nivel_StarsObtained_Cambia()
    {
        var level = new Level();
        level.starsObtained = new EnumStateLevelYStars.LevelYStars[] { EnumStateLevelYStars.LevelYStars.Obtenida, EnumStateLevelYStars.LevelYStars.Provisional };
        Assert.AreEqual(2, level.starsObtained.Length);
        Assert.AreEqual(EnumStateLevelYStars.LevelYStars.Obtenida, level.starsObtained[0]);
        Assert.AreEqual(EnumStateLevelYStars.LevelYStars.Provisional, level.starsObtained[1]);
    }

    [Test]
    public void Nivel_IsLevelCompleted_Cambia()
    {
        var level = new Level();
        level.isLevelCompleted = EnumStateLevelYStars.LevelYStars.Obtenida;
        Assert.AreEqual(EnumStateLevelYStars.LevelYStars.Obtenida, level.isLevelCompleted);
    }

    [Test]
    public void Nivel_EsSerializable()
    {
        var level = new Level { identificadorNivel = "1-2", starsObtained = new[] { EnumStateLevelYStars.LevelYStars.Obtenida }, isLevelCompleted = EnumStateLevelYStars.LevelYStars.Obtenida };
        string json = JsonUtility.ToJson(level);
        var restored = JsonUtility.FromJson<Level>(json);
        Assert.AreEqual("1-2", restored.identificadorNivel);
        Assert.AreEqual(1, restored.starsObtained.Length);
        Assert.AreEqual(EnumStateLevelYStars.LevelYStars.Obtenida, restored.isLevelCompleted);
    }
}
