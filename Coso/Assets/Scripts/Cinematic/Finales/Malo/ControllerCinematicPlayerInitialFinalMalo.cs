using UnityEngine;

/// <summary>
/// Clase que gestiona el controlador del jugador en la cinemática inicial del final malo
/// </summary>
public class ControllerCinematicPlayerInitialFinalMalo : ControllerCinematicPlayerBase
{
    [SerializeField]
    [Tooltip("Ubicacion final del jugador")]
    private Vector2 posicionFinalAsignada;

    protected override Vector2 posicionFinal => posicionFinalAsignada;
}