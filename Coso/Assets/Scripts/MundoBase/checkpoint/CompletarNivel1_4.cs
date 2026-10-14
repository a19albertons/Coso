/// <summary>
/// Clase que se encarga de evaluar el progreso del nivel 1-4 para preguntar si se desea hacer un guardado permanente del progreso
/// </summary>
public class EvaluarNivel1_4 : CompletarNivelBase
{
    /// <summary>
    /// Implementa el metodo abstracto que evalua si el nivel 1-4 ya fue completado y se debe mostrar el canvas de guardado
    /// </summary>
    protected override void Cambiarcheckpoint()
    {
        if (SaveManager.GetIntance().currentSave != null)
        {
            if (!SaveManager.GetIntance().currentSave.checkpoint.level1_4IsDone)
            {
                if (SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)) != null)
                {
                    if (SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)).isLevelCompleted == EnumStateLevelYStars.LevelYStars.Provisional)
                    {
                        SaveManager.GetIntance().currentSave.checkpoint.level1_4IsDone = true;
                        MostrarCanvas();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Se evalua al cargar la escena de mundo base si el nivel 1-4 ya fue completado y se debe mostrar el canvas de guardado
    /// </summary>
    void Awake()
    {
        Cambiarcheckpoint();
    }
}