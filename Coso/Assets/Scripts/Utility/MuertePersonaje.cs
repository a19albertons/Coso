using System.Collections;

/// <summary>
/// Clase MuertePersonaje que gestiona la muerte del personaje y la carga de la escena de forma asincrona para evitar congelar el juego mientras se carga
/// </summary>
public static class MuertePersonaje
{
    /// <summary>
    /// Guarda los datos del jugador cuando muere y cambia la escena en función del numero de vidas
    /// </summary>
    /// <returns></returns>
    public static IEnumerator GuardarYCambiarEscena()
    {
        // Actualizar metricas del jugador
        SaveManager.GetInstance().currentSave.playerInfo.powerUpActual = GestionPowerUP.GetInstance().TipoPowerUP();
        SaveManager.GetInstance().currentSave.playerInfo.vidasActuales = GestionVidas.GetInstance().GetVidasActualesInt();
        SaveManager.GetInstance().currentSave.playerInfo.monedasActuales = GestionMonedas.GetInstance().GetMonedasAcumuladasInt();
        SaveManager.GetInstance().currentSave.checkpoint.haMuerto = true;
        SaveManager.GetInstance().SaveGame(1);
        yield return null;
        if (GestionVidas.GetInstance().GetVidasActualesInt() > 0)
        {
            GestionEscenas.GetInstance().CambiarEscena("MundoBase");
        }
        else
        {
            GestionEscenas.GetInstance().CambiarEscena("HasPerdido");
        }
    }

}