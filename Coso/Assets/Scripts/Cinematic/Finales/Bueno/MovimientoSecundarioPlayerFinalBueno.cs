using UnityEngine;

/// <summary>
/// Clase que gestiona el movimiento secundario del jugador en la cinemática del final bueno
/// </summary>
public class MovimientoSecundarioPlayerFinalBueno : ControllerCinematicPlayerBase
{
    [SerializeField]
    [Tooltip("Nueva ubicación del jugador")]
    private Vector2 nuevaUbicacion;

    protected override Vector2 posicionFinal => nuevaUbicacion;
}