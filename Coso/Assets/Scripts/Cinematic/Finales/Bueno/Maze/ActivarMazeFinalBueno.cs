using UnityEngine;

/// <summary>
/// Clase que gestiona la activación del Maze final bueno, haciendo que caiga sobre la ubicación indicada
/// </summary>
public class ActivarMazeFinalBueno : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ubicación donde caer")]
    private Vector2 posicionCaida;
    private bool moverse = false;

    /// <summary>
    /// Activa el movimiento de la maza hacia la ubicación indicada
    /// </summary>
    public void Activar()
    {
        moverse = true;
    }

    void FixedUpdate()
    {
        Caer();
    }

    /// <summary>
    /// La maza cae sobre la ubicación indicada, si se ha activado el movimiento.
    /// </summary>
    private void Caer()
    {
        if (moverse)
        {
            transform.position = Vector2.MoveTowards(transform.position, posicionCaida, 10f * Time.fixedDeltaTime);
        }
    }
}