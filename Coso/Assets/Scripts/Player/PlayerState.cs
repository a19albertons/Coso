[System.Serializable]
public class PlayerState
{
    public enum PlayerStates
    {
        Ground,
        Jump,
        Ramp,
        Death
    }

    public PlayerStates estadoActual = PlayerStates.Ground;
}