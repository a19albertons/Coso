using UnityEngine;

/// <summary>
/// Clase que gestiona el movimiento secundario del jugador en la cinemática del final malo
/// </summary>
public class MovimientoSecundarioPlayerFinalMalo : ControllerCinematicPlayerBase
{
    [SerializeField]
    [Tooltip("Nueva ubicación del jugador")]
    private Vector2 nuevaUbicacion;

    protected override Vector2 posicionFinal => nuevaUbicacion;
}