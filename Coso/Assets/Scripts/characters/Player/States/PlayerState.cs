/// <summary>
/// Clase base para todos los estados del jugador.
/// </summary>
public abstract class PlayerState
{
    protected readonly PlayerController Player;
    protected PlayerState(PlayerController player) => Player = player;

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}