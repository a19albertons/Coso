using System;
using NUnit.Framework;

/// <summary>
/// EnumStateLevelYStarsEditModeTests es una clase de pruebas unitarias para la clase EnumStateLevelYStars.
/// </summary>
public class EnumStateLevelYStarsEditModeTests
{
    [Test]
    public void LevelYStars_ValuesSonLosEsperados()
    {
        Assert.AreEqual(EnumStateLevelYStars.LevelYStars.NoObtenida, EnumStateLevelYStars.LevelYStars.NoObtenida);
        Assert.AreEqual(EnumStateLevelYStars.LevelYStars.Provisional, EnumStateLevelYStars.LevelYStars.Provisional);
        Assert.AreEqual(EnumStateLevelYStars.LevelYStars.Obtenida, EnumStateLevelYStars.LevelYStars.Obtenida);
    }

    [Test]
    public void LevelYStars_EsEnum()
    {
        var enumValue = EnumStateLevelYStars.LevelYStars.Obtenida;
        Assert.IsAssignableFrom<EnumStateLevelYStars.LevelYStars>(enumValue);
        Assert.AreEqual("NoObtenida", Enum.GetName(typeof(EnumStateLevelYStars.LevelYStars), EnumStateLevelYStars.LevelYStars.NoObtenida));
        Assert.AreEqual("Provisional", Enum.GetName(typeof(EnumStateLevelYStars.LevelYStars), EnumStateLevelYStars.LevelYStars.Provisional));
        Assert.AreEqual("Obtenida", Enum.GetName(typeof(EnumStateLevelYStars.LevelYStars), EnumStateLevelYStars.LevelYStars.Obtenida));
    }

    [Test]
    public void LevelYStars_CanBeUsedInArrays()
    {
        var stars = new[] { EnumStateLevelYStars.LevelYStars.NoObtenida, EnumStateLevelYStars.LevelYStars.Provisional, EnumStateLevelYStars.LevelYStars.Obtenida };
        Assert.AreEqual(3, stars.Length);
    }
}
