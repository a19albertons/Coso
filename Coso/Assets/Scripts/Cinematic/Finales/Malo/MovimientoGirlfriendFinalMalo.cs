using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MovimientoGirlfriendFinalMalo : MonoBehaviour
{
    private bool moverse = false;
    [SerializeField]
    [Tooltip("Punto donde movemos a la girlfriend fuera de camara en el final malo")]
    private Vector2 puntoFueraDeCamara;

    public void IniciarMovimiento()
    {
        moverse = true;
    }

    void FixedUpdate()
    {
        MoverPersonajes();
    }
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