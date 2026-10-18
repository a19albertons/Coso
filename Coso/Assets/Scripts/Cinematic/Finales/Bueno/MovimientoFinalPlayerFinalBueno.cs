using UnityEngine;

public class MvomientoFinalPlayerFinalBueno : ControllerCinematicPlayerBase
{
    [SerializeField]
    [Tooltip("Ubicación del jugador fuera de la cámara")]
    private Vector2 ubicacionFueraCamara;

    protected override Vector2 posicionFinal => ubicacionFueraCamara;
}