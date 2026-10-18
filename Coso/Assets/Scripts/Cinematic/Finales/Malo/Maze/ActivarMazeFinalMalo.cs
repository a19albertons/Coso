using UnityEngine;


public class ActivarMazeFinalMalo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ubicación donde caer")]
    private Vector2 posicionCaida;
    private bool moverse = false;

    public void Activar()
    {
        moverse = true;
    }

    void FixedUpdate()
    {
        CaerSobrePlayer();
    }

    private void CaerSobrePlayer()
    {
        if (moverse)
        {
            transform.position = Vector2.MoveTowards(transform.position, posicionCaida, 10f * Time.fixedDeltaTime);
        }
    }
}