using System.Collections;
using UnityEngine;

/// <summary>
/// Clase que gestiona las operaciones en segundo plano durante la cinemática de introducción, carga el mundo base y hace el primer guardado de datos en el juego.
/// </summary>
public class SegundoPlano : MonoBehaviour
{

    public void LanzarOperacionesSegundoPlano()
    {
        StartCoroutine(PrimerGuardado());
    }

    /// <summary>
    /// Carga la escena de mundo base a través del nuevo gestor de escenas. Asumimos por tiempo que el guardado esta hecho
    /// </summary>
    /// <returns></returns>
    public void CargarMundoBase()
    {
        GestionEscenas.GetInstance().CambiarEscena("MundoBase");
        
    }

    /// <summary>
    /// Guarda los datos del jugador en el primer checkpoint y marca el proceso issaved como verdadero/hecho
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrimerGuardado()
    {
        SaveManager.GetInstance().currentSave.checkpoint.IsTutorialDone = true;
        SaveManager.GetInstance().SaveGame();
        yield return null;
    }
}