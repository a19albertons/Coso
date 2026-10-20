using UnityEngine;

/// <summary>
/// Clase que gestiona el efecto parallax de la imagen de fondo en relación al movimiento del jugador
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Objeto de referencia el personaje")]
    private Transform referencia;

    [SerializeField]
    [Tooltip("Multiplicado reductor en comparación al avance del jugador")]
    [Range(0, 1f)]
    private float parallaxFactor = 0.01f;


    private SpriteRenderer background;

    private Material backgroundMaterial;

    private Vector2 posicionInicial;
    private float posicionJugadorX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        background = GetComponent<SpriteRenderer>();
        backgroundMaterial = background.material;
        posicionJugadorX = referencia.position.x;
        posicionInicial = backgroundMaterial.GetVector("_playerOffset");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x == 1)
        {
            float diferenciaJugadorX = referencia.position.x - posicionJugadorX;

            float nuevoOffsetX = diferenciaJugadorX * parallaxFactor;

            nuevoOffsetX = Mathf.Repeat(nuevoOffsetX, 1.0f);

            backgroundMaterial.SetVector("_playerOffset", new Vector2(nuevoOffsetX, posicionInicial.y));
        }
        else
        {
            float diferenciaJugadorX = referencia.position.x - posicionJugadorX;

            float nuevoOffsetX = diferenciaJugadorX * parallaxFactor / transform.localScale.x;

            nuevoOffsetX = Mathf.Repeat(nuevoOffsetX, 1.0f);

            backgroundMaterial.SetVector("_playerOffset", new Vector2(nuevoOffsetX, posicionInicial.y));
        }

    }
}
