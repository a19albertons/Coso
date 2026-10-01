[System.Serializable]
public class PlayerState
{
    public enum PlayerStates
    {
        Ground,
        Jump,
        Ramp
    }

    public PlayerStates estadoActual = PlayerStates.Ground;
}