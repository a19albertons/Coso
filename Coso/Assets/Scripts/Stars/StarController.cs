using UnityEngine;

/// <summary>
/// Clase que controla las estrellas en el nivel.
/// </summary>
public class StarController: MonoBehaviour
{
    [SerializeField]
    [Tooltip("Número de estrella del 1 al 3. Evitar usar el mismo número para varias estrellas.")]
    [Range(1, 3)]
    private int starNumber;
    private GestionEstrellas levelManager;

    void Awake()
    {
        levelManager = FindAnyObjectByType<GestionEstrellas>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (levelManager != null)
            {
                levelManager.EstrellaObtenida(starNumber);
                gameObject.SetActive(false); // Desactivar la estrella al ser obtenida
            }
            else
            {
                Debug.LogWarning("LevelManager no encontrado en la escena.");
            }
        }
    }
}