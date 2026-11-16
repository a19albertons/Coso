using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionEscenas : MonoBehaviour
{
    private static GestionEscenas instancia;

    public static GestionEscenas GetInstance()
    {
        return instancia;
    }

    public AsyncOperation opStatus { get; private set;}

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instancia != this)
        {
            Destroy(gameObject);
        }
    }

    public void CambiarEscena(string nuevaEscena)
    {
        SceneManager.LoadScene("PantallaCarga");
        StartCoroutine(GestionarEscenaObjetivoAsincronamente(nuevaEscena));
    }

    private IEnumerator GestionarEscenaObjetivoAsincronamente(string nuevaEscena)
    {
        yield return null;
        // Declara la nueva escena para opStatus
        opStatus = SceneManager.LoadSceneAsync(nuevaEscena);
        
    }
}