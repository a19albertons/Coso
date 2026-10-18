using UnityEngine;

/// <summary>
/// Clase que gestiona el movimiento de la girlfriend en la cinemática del final malo
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class MovimientoGirlfriendFinalMalo : MonoBehaviour
{
    private bool moverse = false;
    [SerializeField]
    [Tooltip("Punto donde movemos a la girlfriend fuera de camara en el final malo")]
    private Vector2 puntoFueraDeCamara;

    /// <summary>
    /// Activa el movimiento de la girlfriend hacia el punto fuera de cámara
    /// </summary>
    public void IniciarMovimiento()
    {
        moverse = true;
    }

    void FixedUpdate()
    {
        MoverPersonajes();
    }

    /// <summary>
    /// Gestiona el movimiento de la girlfriend hacia el punto fuera de cámara si se ha activado el movimiento
    /// </summary>
    private void MoverPersonajes()
    {
        if (!moverse)
        {
            return;
        }
        GetComponent<SpriteRenderer>().flipX = false;
        transform.position = Vector2.MoveTowards(transform.position, puntoFueraDeCamara, 3f * Time.fixedDeltaTime);
    }

}