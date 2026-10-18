using System.Collections.Generic;

/// <summary>
/// Clase que representa la partida actual del juego, incluyendo la información del jugador, los niveles y los checkpoints
/// </summary>
[System.Serializable]
public class CurrentSave
{
    public PlayerInfo playerInfo = new PlayerInfo();
    public List<Level> levels = new List<Level>();
    public Checkpoint checkpoint = new Checkpoint();
}