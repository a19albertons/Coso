using UnityEngine;

/// <summary>
/// Clase que gestiona el controlador del jugador en la cinemática inicial
/// </summary>
public class ControllerCinematicPlayerInitial : ControllerCinematicPlayerBase
{
    [SerializeField]
    [Tooltip("Ubicacion final del jugador")]
    private Vector2 posicionFinalAsignada;

    protected override Vector2 posicionFinal => posicionFinalAsignada;
}