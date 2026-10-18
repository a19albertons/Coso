using UnityEngine;


/// <summary>
/// Clase que gestiona la activación del movimiento del maze en la cinemática del final malo
/// </summary>
public class ActivarMazeFinalMalo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ubicación donde caer")]
    private Vector2 posicionCaida;
    private bool moverse = false;

    /// <summary>
    /// Activa el movimiento del maze hacia la ubicación indicada
    /// </summary>
    public void Activar()
    {
        moverse = true;
    }

    void FixedUpdate()
    {
        CaerSobrePlayer();
    }

    /// <summary>
    /// La maza cae sobre la ubicación indicada, si se ha activado el movimiento.
    /// </summary>
    private void CaerSobrePlayer()
    {
        if (moverse)
        {
            transform.position = Vector2.MoveTowards(transform.position, posicionCaida, 10f * Time.fixedDeltaTime);
        }
    }
}