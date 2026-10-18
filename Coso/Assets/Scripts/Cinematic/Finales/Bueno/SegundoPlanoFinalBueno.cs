using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona las corrutinas en segundo plano que se ejecutan durante la cinemática del final bueno, como la carga de la escena "MundoBase"
/// </summary>
public class SegundoPlanoFinalBueno : MonoBehaviour
{
    private bool isCinematicFinished = false; // Variable para controlar si la cinemática ha finalizado

    public void LanzarOperacionesSegundoPlano()
    {
        StartCoroutine(PrepararMundoBase());
    }
    /// <summary>
    /// Prepara la carga de la escena "MundoBase", mientrás se ejecuta la cinemática para ser llamada cuando se haya terminado el guardado y la cinemática inicial
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrepararMundoBase()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("MundoBase");
        op.allowSceneActivation = false; // Evita que la escena se active automáticamente al terminar de cargar
        while (!isCinematicFinished)
        {
            yield return null; // Espera un frame antes de volver a comprobar las condiciones
        }
        op.allowSceneActivation = true; // Provoca el salto de escena una vez se cumplen las condiciones establecidas
        yield return op;
    }

    /// <summary>
    /// Marca el proceso isCinematicFinished como verdadero/hecho
    /// </summary>
    public void CinematicaInicialFinalizada()
    {
        isCinematicFinished = true;
    }
}