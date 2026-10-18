using UnityEngine;

/// <summary>
/// Clase que gestiona el guardado centralizado del progreso del mundo base, consolidando el progreso provisional a obtenido de forma permanente y guardando el progreso en el sistema de ficheros.
/// </summary>
public static class GuardadoCentralizadoMundoBase
{
        /// <summary>
    /// Realiza los cambios de estado provisional a obtenido de forma permanente
    /// </summary>
    public static void ConsolidarProgreso()
    {
        if (SaveManager.GetIntance().currentSave == null)
        {
            Debug.LogError("No hay un guardado activo para aplicar el progreso del nivel");
            return;
        }
        // Se recorre la lista de niveles del guardado actual y se actualiza el estado de los niveles y estrellas obtenidas
        SaveManager.GetIntance().currentSave.levels.ForEach(level =>
        {
            // Guardar de forma permanente el progreso obtenido
            if (level.isLevelCompleted == EnumStateLevelYStars.LevelYStars.Provisional)
            {
                level.isLevelCompleted = EnumStateLevelYStars.LevelYStars.Obtenida;
            }
            // Actualizar el estado de las estrellas
            for (int i = 0; i < level.starsObtained.Length; i++)
            {
                if (level.starsObtained[i] == EnumStateLevelYStars.LevelYStars.Provisional)
                {
                    level.starsObtained[i] = EnumStateLevelYStars.LevelYStars.Obtenida;
                }
            }
        });
    }

    /// <summary>
    /// Se guarda el progreso en el fichero asociado en el sistema de ficheros.
    /// </summary>
    public static void GuardarProgreso()
    {
        if (SaveManager.GetIntance().currentSave == null)
        {
            Debug.LogError("No hay un guardado activo para aplicar el progreso del nivel");
            return;
        }
        SaveManager.GetIntance().currentSave.playerInfo.monedasActuales = GestionMonedas.GetInstance().GetMonedasAcumuladasInt();
        SaveManager.GetIntance().currentSave.playerInfo.vidasActuales = GestionVidas.GetInstance().GetVidasActualesInt();
        SaveManager.GetIntance().currentSave.playerInfo.powerUpActual = GestionPowerUP.GetInstance().TipoPowerUP();
        SaveManager.GetIntance().SaveGame(1);
    }
}