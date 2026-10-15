using UnityEngine;

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