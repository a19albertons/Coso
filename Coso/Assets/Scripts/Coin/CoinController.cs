using UnityEngine;

public class CoinController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Incrementa el contador de monedas en GeneralManager
            GeneralManager generalManager = GeneralManager.GetInstance();
            if (generalManager != null)
            {
                GeneralManager.GetInstance().SumarMoneda();
            }

            // Destruye la moneda
            Destroy(gameObject);
        }
    }
}