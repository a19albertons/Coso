using UnityEngine;

/// <summary>
/// Clase que gestiona la finalización del nivel 1-8 Parte 2, verificando si el jugador ha completado el nivel y actualizando el checkpoint correspondiente donde el juego se da por completado.
/// </summary>
public class CompletarNivel1_8Parte2 : CompletarNivelBase
{
    protected override void Cambiarcheckpoint()
    {
        if (SaveManager.GetInstance().currentSave != null)
        {
            if (SaveManager.GetInstance().currentSave.checkpoint.level1_8IsDone)
            {
                if (SaveManager.GetInstance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)) != null)
                {
                    if (SaveManager.GetInstance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)).isLevelCompleted == EnumStateLevelYStars.LevelYStars.Obtenida)
                    {
                        if (!SaveManager.GetInstance().currentSave.checkpoint.JuegoCompletado)
                        {
                            SaveManager.GetInstance().currentSave.checkpoint.JuegoCompletado = true;
                            MostrarCanvas();
                        }
                    }
                }
            }
        }
    }

    void Awake()
    {
        Cambiarcheckpoint();
    }
}