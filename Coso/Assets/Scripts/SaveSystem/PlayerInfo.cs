/// <summary>
/// Alberga la información importante asociado al jugador, incluyendo las vidas actuales, las monedas actuales y el power-up actual.
/// </summary>
[System.Serializable]
public class PlayerInfo
{
    public int vidasActuales = 5; // Establece las vidas iniciales
    public int monedasActuales = 0; // Establece las monedas iniciales
    public PowerUPType powerUpActual = PowerUPType.None; // Establece el power-up inicial
}