/// <summary>
/// Representa un nivel del juego, incluyendo su identificador, las estrellas obtenidas y si el nivel ha sido completado o no.
/// </summary>
[System.Serializable]
public class Level
{
    public string identificadorNivel;
    public EnumStateLevelYStars.LevelYStars[] starsObtained;
    public EnumStateLevelYStars.LevelYStars isLevelCompleted;
    
}