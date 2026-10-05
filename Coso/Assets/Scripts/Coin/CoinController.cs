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