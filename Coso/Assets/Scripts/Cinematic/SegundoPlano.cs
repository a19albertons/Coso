using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SegundoPlano : MonoBehaviour
{
    private bool isSaved = false; // Variable para controlar si los datos del jugador ya han sido guardados
    private bool isCinematicFinished = false; // Variable para controlar si la cinemática ha finalizado

    public void LanzarOperacionesSegundoPlano()
    {
        StartCoroutine(PrimerGuardado());
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
        while (!isSaved || !isCinematicFinished)
        {
            yield return null; // Espera un frame antes de volver a comprobar las condiciones
        }
        op.allowSceneActivation = true; // Provoca el salto de escena una vez se cumplen las condiciones establecidas
        yield return op;
    }

    /// <summary>
    /// Guarda los datos del jugador en el primer checkpoint y marca el proceso issaved como verdadero/hecho
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrimerGuardado()
    {
        SaveManager.GetIntance().currentSave.checkpoint.IsTutorialDone = true;
        SaveManager.GetIntance().SaveGame(1);
        isSaved = true;
        yield return null;
    }

    /// <summary>
    /// Marca el proceso isCinematicFinished como verdadero/hecho
    /// </summary>
    public void CinematicaInicialFinalizada()
    {
        isCinematicFinished = true;
    }
}