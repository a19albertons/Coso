using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(BoxCollider2D))]
public class MazeMovement : MonoBehaviour
{
    // Configuraciones de velocidad
    [SerializeField]
    [Tooltip("La velocidad al que personaje vuelve a su posición original")]
    private float ascendentSpeed = 3f;

    [SerializeField]
    [Tooltip("La velocidad con la que el personaje cae a plomo")]
    private float descendentsSpeed = 10f;

    // Rango de detección del jugador
    [SerializeField]
    [Tooltip("La distancia a la que el personaje detecta al jugador")]
    private float numBloques = 3f;
    private float DistanciaDeteccion => numBloques * 2f;

    [SerializeField]
    [Tooltip("El tiempo que el enemigo espera antes de ascender")]
    private float tiempoEsperaAscenso = 1f;

    [SerializeField]
    [Tooltip("El tiempo que espera el enemigo antes de volver a caer")]
    private float tiempoEsperaCaida = 1f;

    // Estrucutras a obtener o calcular del escenario o objeto

    private RaycastHit2D fondo;
    private RaycastHit2D deteccionPlayer;
    private LayerMask layerMaskFloor;
    private LayerMask layerMaskPlayer;
    private new Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    // Posiciones y distancias
    private float sizeYMitad;
    private float distanciaAlSueloInspeccion;
    private Vector2 posicionOriginal;
    private Vector2 posicionFinal;

    // estados
    private bool descendiendo = false;
    private bool ascendiendo = false;

    // delays
    private float delayAscenso;
    private float delayDescenso;
    private bool primeraCaida = true;

    void Awake()
    {
        posicionOriginal = transform.position;
        distanciaAlSueloInspeccion = 20f;
        layerMaskFloor = LayerMask.GetMask("Floor");
        layerMaskPlayer = LayerMask.GetMask("Player");
        fondo = Physics2D.Raycast(posicionOriginal, Vector2.down, distanciaAlSueloInspeccion, layerMaskFloor);
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        sizeYMitad = boxCollider.size.y / 2f;
        posicionFinal = new(posicionOriginal.x, fondo.point.y + sizeYMitad);
    }

    void Update()
    {
        Debug.DrawRay(posicionOriginal, Vector2.down * distanciaAlSueloInspeccion, Color.red);
    }

    void FixedUpdate()
    {
        // Detección del jugador
        deteccionPlayer = Physics2D.BoxCast(posicionOriginal + new Vector2(0f, -distanciaAlSueloInspeccion / 2f), new Vector2(DistanciaDeteccion, distanciaAlSueloInspeccion), 0f, Vector2.right, 0f, layerMaskPlayer);
        // Gestiona las condiciones para la caida del personaje
        if (deteccionPlayer.collider != null && ascendiendo == false && (primeraCaida || delayDescenso > tiempoEsperaCaida))
        {

            rigidbody.MovePosition(Vector2.MoveTowards(transform.position, posicionFinal, descendentsSpeed * Time.fixedDeltaTime));
            descendiendo = true;
            primeraCaida = false;
        }
        // Valida si se ha llegado o no al punto final 
        if (descendiendo && Vector2.Distance(transform.position, posicionFinal) < 0.1f)
        {
            descendiendo = false;
            ascendiendo = true;
            delayAscenso = 0f;
        }
        // Llegado al punto final de caida, hace una espera antes de volver a subir
        if (ascendiendo)
        {
            delayAscenso += Time.fixedDeltaTime;
            if (delayAscenso > tiempoEsperaAscenso)
            {
                rigidbody.MovePosition(Vector2.MoveTowards(transform.position, posicionOriginal, ascendentSpeed * Time.fixedDeltaTime));
                if (Vector2.Distance(transform.position, posicionOriginal) < 0.1f)
                {
                    ascendiendo = false;
                    delayDescenso = 0f;
                }
            }
        }
        // Gestiona la condicion especifica a partir de la segunda caida, para que el personaje espere un tiempo antes de volver a caer
        if (primeraCaida || !ascendiendo && !descendiendo)
        {
            delayDescenso += Time.fixedDeltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(posicionOriginal + new Vector2(0f, -distanciaAlSueloInspeccion / 2f), new Vector3(DistanciaDeteccion, distanciaAlSueloInspeccion, 0f));
    }
}
