/// <summary>
/// Clase que se encarga de evaluar el progreso del nivel 1-8 para preguntar si se desea hacer un guardado permanente del progreso
/// </summary>
public class EvaluarNivel1_8 : CompletarNivelBase
{
    /// <summary>
    /// Implementa el metodo abstracto que evalua si el nivel 1-8 ya fue completado y se debe mostrar el canvas de guardado
    /// </summary>
    protected override void Cambiarcheckpoint()
    {
        if (SaveManager.GetInstance().currentSave != null)
        {
            if (!SaveManager.GetInstance().currentSave.checkpoint.level1_8IsDone)
            {
                if (SaveManager.GetInstance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)) != null)
                {
                    if (SaveManager.GetInstance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)).isLevelCompleted == EnumStateLevelYStars.LevelYStars.Provisional)
                    {
                        SaveManager.GetInstance().currentSave.checkpoint.level1_8IsDone = true;
                        LanzarGuardado();
                        // Evaluamos si el jugador ha muerto alguna vez para mostrar un final u otro
                        if (SaveManager.GetInstance().currentSave.checkpoint.haMuerto)
                        {
                            FinalMalo();
                        }
                        else
                        {
                            FinalBueno();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gestiona un guardado automático de los datos
    /// </summary>
    /// <returns></returns>
    private void LanzarGuardado()
    {
        GuardadoCentralizadoMundoBase.ConsolidarProgreso();
        GuardadoCentralizadoMundoBase.GuardarProgreso();
        SaveManager.GetInstance().SaveGame(1);
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
        GestionEscenas.GetInstance().CambiarEscena("FinalMalo");
    }

    /// <summary>
    /// Gestiona el final bueno del juego.
    /// </summary>
    private void FinalBueno()
    {
        GestionEscenas.GetInstance().CambiarEscena("FinalBueno");
    }
}