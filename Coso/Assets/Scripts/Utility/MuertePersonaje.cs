using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase MuertePersonaje que gestiona la muerte del personaje y la carga de la escena de forma asincrona para evitar congelar el juego mientras se carga
/// </summary>
public static class MuertePersonaje
{
    /// <summary>
    /// Variable de espera de 0.1 segundos para simular el tiempo de espera antes de comprobar si el guardado ha finalizado
    /// </summary>
    private static WaitForSeconds _waitForSeconds0_1 = new(0.1f);

    /// <summary>
    /// Variable para controlar si los datos del jugador ya han sido guardados
    /// </summary>
    private static bool isSaved = false; // Variable para controlar si los datos del jugador ya han sido guardados

    /// <summary>
    /// Guarda los datos del jugador en los casos de muerte del personaje
    /// </summary>
    /// <returns></returns>
    public static IEnumerator GuardarDatosJugador()
    {
        // Actualizar metricas del jugador
        SaveManager.GetIntance().currentSave.playerInfo.powerUpActual = GestionPowerUP.GetInstance().TipoPowerUP();
        SaveManager.GetIntance().currentSave.playerInfo.vidasActuales = GestionVidas.GetInstance().GetVidasActualesInt();
        SaveManager.GetIntance().currentSave.playerInfo.monedasActuales = GestionMonedas.GetInstance().GetMonedasAcumuladasInt();
        SaveManager.GetIntance().SaveGame(1);
        isSaved = true; // Marca que los datos del jugador han sido guardados
        yield return null; ;
    }

    /// <summary>
    /// Gestiona la carga de la escena de forma asincrona para evitar congelar el juego mientras se carga
    /// </summary>
    /// <returns></returns>
    public static IEnumerator CargarEscenaAsync()
    {
        AsyncOperation op;
        if (GestionVidas.GetInstance().GetVidasActualesInt() > 0)
        {
            op = SceneManager.LoadSceneAsync("MundoBase");
        }
        else
        {
            op = SceneManager.LoadSceneAsync("HasPerdido");
        }
        op.allowSceneActivation = false; // Evita que la escena se active automáticamente al terminar de cargar
        while (!isSaved)
        {
            yield return _waitForSeconds0_1; // Espera 100 milisegundos antes de volver a comprobar si el guardado hafinalizado
        }

        isSaved = false; // Reinicia la variable para futuros usos
        op.allowSceneActivation = true; // Permite que la escena se active una vez que se haya cargado completamente

        yield return op;

    }

}