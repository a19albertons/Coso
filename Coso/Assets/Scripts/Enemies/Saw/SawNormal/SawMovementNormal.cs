using UnityEngine;

/// <summary>
/// Clase que gestiona el movimiento de la sierra normal a través de puntos de movimiento en un eje 2D.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class SawMovementNormal : SawMovementBase
{
    [SerializeField]
    [Tooltip("Puntos por los que se mueve la sierra")]
    private Transform[] puntosMovimiento;

    private LineRenderer linea;
    private int puntoActual = 0;

    void Awake()
    {
        // Evalua si existen puntos en caso negativo se queda estatica donde se ha declarado en la escena.
        if (puntosMovimiento.Length >=1)
        {
            transform.position = puntosMovimiento[0].position;
        }
        else
        {
            Debug.LogWarning("No hay puntos asignados en la sierra " + gameObject.name + " revise si son necesarios. Se queda estatica en la posición declarada en la escena como callback.");
        }
        linea = GetComponent<LineRenderer>();
        linea.positionCount = puntosMovimiento.Length;
        for (int i = 0; i < puntosMovimiento.Length; i++)
        {
            linea.SetPosition(i, puntosMovimiento[i].position);
        }
        linea.loop = true;
    }


    protected override void MoverSierra()
    {
        if (puntosMovimiento.Length == 0)
        {
            Debug.Log("No se han asignado puntos de movimiento para la sierra. Se mantiene estatica en la posición declarada.");
            return;
        }

        Transform puntoDestino = puntosMovimiento[puntoActual];
        transform.position = Vector2.MoveTowards(transform.position, puntoDestino.position, velocidad * Time.fixedDeltaTime);

        if (Vector2.Distance(transform.position, puntoDestino.position) < 0.1f)
        {
            puntoActual = (puntoActual + 1) % puntosMovimiento.Length;
        }
    }
}