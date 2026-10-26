using UnityEngine;

/// <summary>
/// Clase que alterna el sprite de fondo en función de la relación de aspecto de la pantalla.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class AlternarBackground : MonoBehaviour
{
    [SerializeField]
    private float aspectRatioThreshold = 1.78f; // Umbral de relación de aspecto para alternar el sprite (16:9 = 1.77)

    [SerializeField]
    private Sprite spriteAlternativo; // Spriter alternativo para pantallas con relación de aspecto mayor a 16:9

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        float tamanoEjeYOriginal = spriteRenderer.bounds.size.y;
        float aspectRatioActual = (float)Screen.width / (float)Screen.height;

        if (aspectRatioActual > aspectRatioThreshold)
        {
            spriteRenderer.sprite = spriteAlternativo;
            if (spriteRenderer.sprite != null && spriteRenderer.bounds.size.y != 0)
            {
                transform.localScale = transform.localScale * (tamanoEjeYOriginal / spriteRenderer.bounds.size.y);
            }
        }
    }
}