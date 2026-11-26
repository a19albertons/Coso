using UnityEngine;

public class ControladorPlataforma : MonoBehaviour
{
    [SerializeField]
    private GameObject[] exclusivoStandlone;


    void Awake()
    {
        #if UNITY_STANDALONE
        {
            foreach(var gameObject in exclusivoStandlone) {
                gameObject.SetActive(true);
            }
        }
        #else
        {
            foreach(var gameObject in exclusivoStandlone) {
                gameObject.SetActive(false);
            }
        }
        #endif
    }
}