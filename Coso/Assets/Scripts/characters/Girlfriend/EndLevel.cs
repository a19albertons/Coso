using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(SpriteRenderer))]
public class EndLevel : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    private static WaitForSeconds _waitForSeconds0_1 = new WaitForSeconds(0.1f);
    [SerializeField]
    [Tooltip("Un punto hacia la derecha fuera de camara para mover al personaje")]
    private Transform movePoint;
    private Vector2 posicionMovePoint;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool isSaved = false; // Variable para controlar si los datos del jugador ya han sido guardados
    private bool moverPersonaje = false; // Variable para controlar si el personaje debe moverse fuera de la cámara
    private bool isMoved = false; // Variable para controlar si el personaje ya ha sido movido fuera de la cámara

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        posicionMovePoint = new(movePoint.position.x, movePoint.position.y); // Guardamos una copia de la posición original independiente en memoria
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Le quitamos la posibilidad al jugador de moverse y saltar
            other.gameObject.GetComponent<PlayerController>().enabled = false;
            other.gameObject.GetComponent<PlayerVisuals>().StopAnimacion();
            other.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Detiene cualquier movimiento del jugador
            // Cargar escena de forma asincrona para evitar congelar el juego mientras se carga
            StartCoroutine(CargarEscenaAsync());
            // Realizar guardado de datos del jugador
            StartCoroutine(GuardarDatosJugador());
            // Mueve al personaje fuera de la camara mientras se carga la escena
            moverPersonaje = true;
            sr.flipX = false; // Vuelve a la posicion original del sprite del personaje

        }
    }

    private IEnumerator GuardarDatosJugador()
    {
        FindAnyObjectByType<ManagerGuardarNivel>().GuardarNivel();
        isSaved = true; // Marca que los datos del jugador han sido guardados
        yield return null;
    }

    private IEnumerator CargarEscenaAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("MundoBase");
        op.allowSceneActivation = false; // Evita que la escena se active automáticamente al terminar de cargar
        while (!isSaved || !isMoved)
        {
            yield return _waitForSeconds0_1; // Espera 100 milisegundos antes de volver a comprobar si el guardado y el movimiento del personaje han finalizado
        }
        op.allowSceneActivation = true; // Permite que la escena se active una vez que se haya cargado completamente

        yield return op;

    }

    private void MoverPersonajeFueraCamara()
    {
        Vector2 posicionFinal = Vector2.MoveTowards(rb.position, posicionMovePoint, 5f * Time.fixedDeltaTime);
        rb.MovePosition(posicionFinal);
    }
    void FixedUpdate()
    {
        if (moverPersonaje)
        {
            MoverPersonajeFueraCamara();
        }
        if (transform.position.Equals(posicionMovePoint))
        {
            moverPersonaje = false;
            isMoved = true;
        }
    }
}