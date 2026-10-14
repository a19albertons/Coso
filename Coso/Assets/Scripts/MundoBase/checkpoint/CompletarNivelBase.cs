using UnityEngine;

/// <summary>
/// Clase base que se encarga de gestionar el canvas de guardado al alcanzar algún checkpoint de nivel como el 1-4
/// </summary>
public abstract class CompletarNivelBase : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Canvas que se mostrara al completar el nivel")]
    private GameObject canvasCompletarNivel;

    [SerializeField]
    [Tooltip("Nombre de la escena que corresponda al checkpoint que se debe evaluar")]
    protected string nombreEscena;

    /// <summary>
    /// Se encarga de mostrar el canvas de guardado al completar el nivel
    /// </summary>
    protected virtual void MostrarCanvas()
    {
        canvasCompletarNivel.SetActive(true);
    }

    /// <summary>
    /// Metodo a implementar por las clases hijas deben integrar la lógica donde se cambia en el guardado el checkpoint que corresponda
    /// </summary>
    protected abstract void Cambiarcheckpoint();
}