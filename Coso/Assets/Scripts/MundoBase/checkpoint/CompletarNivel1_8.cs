using System.Collections;
using UnityEngine;


/// <summary>
/// Clase que se encarga de evaluar el progreso del nivel 1-8 para preguntar si se desea hacer un guardado permanente del progreso
/// </summary>
public class EvaluarNivel1_8 : CompletarNivelBase
{
    private static WaitForSeconds _waitForSeconds0_1 = new WaitForSeconds(0.1f);

    /// <summary>
    /// Variable que evalua si ya hemos guardado los datos del jugador al terminar el juego.
    /// </summary>
    private bool isSaved = false;

    /// <summary>
    /// Implementa el metodo abstracto que evalua si el nivel 1-8 ya fue completado y se debe mostrar el canvas de guardado
    /// </summary>
    protected override void Cambiarcheckpoint()
    {
        if (SaveManager.GetIntance().currentSave != null)
        {
            if (!SaveManager.GetIntance().currentSave.checkpoint.level1_8IsDone)
            {
                if (SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)) != null)
                {
                    if (SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)).isLevelCompleted == EnumStateLevelYStars.LevelYStars.Provisional)
                    {
                        // Evaluamos si el jugador ha muerto alguna vez para mostrar un final u otro
                        if (SaveManager.GetIntance().currentSave.checkpoint.haMuerto)
                        {
                            FinalMalo();
                        }
                        else
                        {
                            FinalBueno();
                        }
                        SaveManager.GetIntance().currentSave.checkpoint.level1_8IsDone = true;
                        StartCoroutine(LanzarGuardado());
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gestiona un guardado automático de los datos
    /// </summary>
    /// <returns></returns>
    private IEnumerator LanzarGuardado()
    {
        GuardadoCentralizadoMundoBase.ConsolidarProgreso();
        GuardadoCentralizadoMundoBase.GuardarProgreso();
        SaveManager.GetIntance().SaveGame(1);
        isSaved = true; // Se marca que los datos del jugador han sido guardados
        yield return null;
    }

    /// <summary>
    /// Se encarga de cargar la escena de final bueno o final malo dependiendo de si el jugador ha muerto alguna vez o no
    /// </summary>
    /// <param name="nombreEscena">nombre en formato string de la escena a cargar</param>
    /// <returns></returns>
    private IEnumerator LanzarEscenaCinematicaFinal(string nombreEscena)
    {
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nombreEscena);
        op.allowSceneActivation = false; // Evita que la escena se active automáticamente al terminar de cargar
        while (!isSaved)
        {
            yield return _waitForSeconds0_1; // Espera 100 milisegundos antes de volver a comprobar si el guardado ha finalizado
        }

        op.allowSceneActivation = true; // Permite que la escena se active una vez que se haya cargado completamente

        yield return op;
    }

    /// <summary>
    /// Se evalua al cargar la escena de mundo base si el nivel 1-8 ya fue completado y se debe mostrar el canvas de guardado
    /// </summary>
    void Awake()
    {
        Cambiarcheckpoint();
    }

    /// <summary>
    /// Gestiona el final malo del juego.
    /// </summary>
    private void FinalMalo()
    {
        StartCoroutine(LanzarEscenaCinematicaFinal("FinalMalo"));
    }

    /// <summary>
    /// Gestiona el final bueno del juego.
    /// </summary>
    private void FinalBueno()
    {
        StartCoroutine(LanzarEscenaCinematicaFinal("FinalBueno"));
    }
}