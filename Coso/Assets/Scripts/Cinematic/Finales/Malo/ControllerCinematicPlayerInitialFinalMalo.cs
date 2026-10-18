using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(PlayerVisuals))]
[RequireComponent(typeof(GroundDetector))]
public class ControllerCinematicPlayerInitialFinalMalo : ControllerCinematicPlayerBase
{
    [SerializeField]
    [Tooltip("Ubicacion final del jugador")]
    private Vector2 posicionFinalAsignada;

    protected override Vector2 posicionFinal => posicionFinalAsignada;
}