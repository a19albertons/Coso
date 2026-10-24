using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestiona el guardado de la información del nivel actual, incluyendo las métricas del jugador y el estado del nivel.
/// </summary>
public class ManagerGuardarNivel : MonoBehaviour
{
    private string idNivel;
    private GestionEstrellas gestionEstrellas;

    void Awake()
    {
        // El identificado de nivel es el nombre de su escena asignada
        idNivel = gameObject.scene.name;
        gestionEstrellas = FindAnyObjectByType<GestionEstrellas>();
        if (gestionEstrellas == null)
        {
            Debug.LogError("Se requiere tener las clase GestionEstrellas en la escena para poder guardar el nivel.");
        }
    }

    public void GuardarNivel()
    {
        // Actualizar metricas del jugador
        SaveManager.GetInstance().currentSave.playerInfo.powerUpActual = GestionPowerUP.GetInstance().TipoPowerUP();
        SaveManager.GetInstance().currentSave.playerInfo.vidasActuales = GestionVidas.GetInstance().GetVidasActualesInt();
        SaveManager.GetInstance().currentSave.playerInfo.monedasActuales = GestionMonedas.GetInstance().GetMonedasAcumuladasInt();

        // Crear el nivel
        Level nivelActual = new Level();
        nivelActual.identificadorNivel = idNivel;
        nivelActual.starsObtained = gestionEstrellas.InformaciónTodasLasEstrellas();
        nivelActual.isLevelCompleted = EnumStateLevelYStars.LevelYStars.Provisional;
        
        List<Level> nivelesGuardados = SaveManager.GetInstance().currentSave.levels;
        for (int i = 0; i < nivelesGuardados.Count; i++)
        {
            if (nivelesGuardados[i].identificadorNivel.Equals(idNivel))
            {
                nivelesGuardados[i] = nivelActual;
                SaveManager.GetInstance().currentSave.levels = nivelesGuardados;
                SaveManager.GetInstance().SaveGame(1);
                return;
            }
        }
        nivelesGuardados.Add(nivelActual);
        SaveManager.GetInstance().currentSave.levels = nivelesGuardados;
        SaveManager.GetInstance().SaveGame(1);
    }
}