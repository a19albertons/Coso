using System.Collections.Generic;

[System.Serializable]
public class CurrentSave
{
    public PlayerInfo playerInfo = new PlayerInfo();
    public List<Level> levels = new List<Level>();
    public Checkpoint checkpoint = new Checkpoint();
}