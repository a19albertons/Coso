using UnityEngine;

/// <summary>
/// Clase que gestiona el movimiento final del jugador en la cinemática del final bueno
/// </summary>
public class MvomientoFinalPlayerFinalBueno : ControllerCinematicPlayerBase
{
    [SerializeField]
    [Tooltip("Ubicación del jugador fuera de la cámara")]
    private Vector2 ubicacionFueraCamara;

    protected override Vector2 posicionFinal => ubicacionFueraCamara;
}