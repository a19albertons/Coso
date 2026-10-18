/// <summary>
/// Clase abstracta que representa los estados de los powerups del jugador, se debe heredar de esta clase para crear un nuevo powerup
/// </summary>
public abstract class PowerUPStates
{
    protected readonly PlayerController Player;

    public abstract PowerUPType PowerUPType { get; }

    protected PowerUPStates(PlayerController player)
    {
        Player = player;
    }
    public virtual void Enter() { }

    public virtual void Exit() { }

    /// <summary>
    /// Metodo que se ejecuta cuando el jugador salta, se debe modificar para configurar la lógica de salto del jugador dependiendo del powerup que tenga activo
    /// </summary>
    public virtual void Jump() { }

    /// <summary>
    /// Metodo que se ejecuta cuando el jugador aplica un cut jump, se debe modificar para configurar la lógica de cut jump del jugador dependiendo del powerup que tenga activo
    /// </summary>
    public virtual void AplicarCutJump() { }
}