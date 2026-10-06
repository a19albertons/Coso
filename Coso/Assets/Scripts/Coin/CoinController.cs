using UnityEngine;

public class CoinController : MonoBehaviour
{
    bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return; // Evita que se recoja la moneda más de una vez
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            if (GestionMonedas.GetInstance() != null)
            {
                GestionMonedas.GetInstance().SumarMoneda();
            }

            // Destruye la moneda
            Destroy(gameObject);
        }
    }
}