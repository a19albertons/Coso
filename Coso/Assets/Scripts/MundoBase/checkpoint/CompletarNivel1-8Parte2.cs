using UnityEngine;

/// <summary>
/// Clase que gestiona la finalización del nivel 1-8 Parte 2, verificando si el jugador ha completado el nivel y actualizando el checkpoint correspondiente donde el juego se da por completado.
/// </summary>
public class CompletarNivel1_8Parte2 : CompletarNivelBase
{
    protected override void Cambiarcheckpoint()
    {
        if (SaveManager.GetIntance().currentSave != null)
        {
            if (SaveManager.GetIntance().currentSave.checkpoint.level1_8IsDone)
            {
                if (SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)) != null)
                {
                    if (SaveManager.GetIntance().currentSave.levels.Find(Level => Level.identificadorNivel.Equals(nombreEscena)).isLevelCompleted == EnumStateLevelYStars.LevelYStars.Obtenida)
                    {
                        if (!SaveManager.GetIntance().currentSave.checkpoint.JuegoCompletado)
                        {
                            SaveManager.GetIntance().currentSave.checkpoint.JuegoCompletado = true;
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