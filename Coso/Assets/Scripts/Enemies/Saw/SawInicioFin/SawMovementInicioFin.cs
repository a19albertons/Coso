using UnityEngine;

/// <summary>
/// Clase que gestiona el movimiento de la sierra desde un punto inicial hasta un punto final en un eje 2D.
/// </summary>
public class SawMovementInicioFin : SawMovementBase
{
    [SerializeField]
    [Tooltip("Punto final de desplazamiento en un eje 2d")]
    private Vector2 puntoFinal;


    protected override void MoverSierra()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoFinal, velocidad * Time.fixedDeltaTime);
    }
}