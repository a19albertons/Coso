using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SawMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Velocidad de movimiento de la sierra")]
    private float velocidad = 3f;

    [SerializeField]
    [Tooltip("Puntos por los que se mueve la sierra")]
    private Transform[] puntosMovimiento;

    private int puntoActual = 0;

    private LineRenderer linea;

    void Awake()
    {
        transform.position = puntosMovimiento[0].position;
        linea = GetComponent<LineRenderer>();
        linea.positionCount = puntosMovimiento.Length;
        for (int i = 0; i < puntosMovimiento.Length; i++)
        {
            linea.SetPosition(i, puntosMovimiento[i].position);
        }
        linea.loop = true;
    }

    void FixedUpdate()
    {
        MoverSierra();
    }


    void MoverSierra()
    {
        if (puntosMovimiento.Length == 0)
        {
            Debug.Log("No se han asignado puntos de movimiento para la sierra.");
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
