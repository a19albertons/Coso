using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Header("Configuraciones")]
    [SerializeField]
    private LayerMask floorLayer;

    [SerializeField]
    [Tooltip("Collider que detecta si estamos tocando el suelo o no")]
    private BoxCollider2D posicionDetectorSuelo;

    [Header("Raycast para detectar pendientes")]
    [SerializeField] private float rayoCentral = 0.8f;
    [SerializeField] private float rayoIzquierdo = 0.52f;
    [SerializeField] private float rayoDerecho = 0.52f;

    [Header("Pendiente")]
    [Tooltip("Angulo minimo en grados para considerar que estamos en una pendiente")]
    [SerializeField] private float anguloPendienteMinimo = 1f;

    private bool suelo;

    // Variables para exponer al PlayerController
    public bool EnTierra { get; private set;}
    public Vector2 NormalTerreno { get; private set; }
    public float AnguloPendiente { get; private set; }
    public bool IsOnSlope => AnguloPendiente > anguloPendienteMinimo;

    // Raycast para detectar pendientes
    private RaycastHit2D hit, hitIzquierda, hitDerecha;

    
    public void ActualizarEstado()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, floorLayer);
        hitIzquierda = Physics2D.Raycast(transform.position + new Vector3(-0.55f, 0f, 0f), Vector2.down, 0.52f, floorLayer);
        hitDerecha = Physics2D.Raycast(transform.position + new Vector3(0.55f, 0f, 0f), Vector2.down, 0.52f, floorLayer);
        EnTierra = Physics2D.OverlapBox(posicionDetectorSuelo.bounds.center, posicionDetectorSuelo.bounds.size * 0.95f, 0f, floorLayer);

        NormalTerreno = EnTierra ? NormalMedia() : Vector2.up;
        AnguloPendiente = Vector2.Angle(NormalTerreno, Vector2.up);
    }
    private Vector2 NormalMedia()
    {

        Vector2 normalMedia = Vector2.zero;
        int count = 0;
        if (hit.collider != null)
        {
            normalMedia += hit.normal;
            count++;
        }
        if (hitIzquierda.collider != null)
        {
            normalMedia += hitIzquierda.normal;
            count++;
        }
        if (hitDerecha.collider != null)
        {
            normalMedia += hitDerecha.normal;
            count++;
        }
        if (count > 0)
        {
            normalMedia /= count;
            return normalMedia.normalized;
        }
        return Vector2.up;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * 0.8f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + new Vector3(-0.55f, 0f, 0f), Vector2.down * 0.52f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + new Vector3(0.55f, 0f, 0f), Vector2.down * 0.52f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(posicionDetectorSuelo.bounds.center, posicionDetectorSuelo.bounds.size * 0.95f);
    }
}