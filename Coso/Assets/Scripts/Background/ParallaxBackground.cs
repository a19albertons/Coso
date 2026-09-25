using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Objeto de referencia el personaje")]
    private Transform referencia;

    [SerializeField]
    [Tooltip("Multiplicado reductor en comparación al avance del jugador")]
    [Range(0,1f)]
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
        float diferenciaJugadorX = referencia.position.x - posicionJugadorX;

        float nuevoOffsetX = diferenciaJugadorX * parallaxFactor;

        nuevoOffsetX = Mathf.Repeat(nuevoOffsetX, 1.0f);

        backgroundMaterial.SetVector("_playerOffset", new Vector2(nuevoOffsetX, posicionInicial.y));
    }
}
