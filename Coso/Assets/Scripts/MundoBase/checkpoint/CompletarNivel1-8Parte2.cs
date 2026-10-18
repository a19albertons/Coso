using UnityEngine;

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