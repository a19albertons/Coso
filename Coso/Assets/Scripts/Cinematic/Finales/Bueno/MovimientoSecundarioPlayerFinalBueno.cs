using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(PlayerVisuals))]
[RequireComponent(typeof(GroundDetector))]
public class MovimientoSecundarioPlayerFinalBueno : ControllerCinematicPlayerBase
{
    [SerializeField]
    [Tooltip("Nueva ubicación del jugador")]
    private Vector2 nuevaUbicacion;

    protected override Vector2 posicionFinal => nuevaUbicacion;
}