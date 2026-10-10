using System.Collections.Generic;
using UnityEngine;

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
        // Crear el nivel
        Level nivelActual = new Level();
        nivelActual.identificadorNivel = idNivel;
        nivelActual.starsObtained = gestionEstrellas.InformaciónTodasLasEstrellas();
        nivelActual.isLevelCompleted = EnumStateLevelYStars.LevelYStars.Provisional;
        
        List<Level> nivelesGuardados = SaveManager.GetIntance().currentSave.levels;
        for (int i = 0; i < nivelesGuardados.Count; i++)
        {
            if (nivelesGuardados[i].identificadorNivel.Equals(idNivel))
            {
                nivelesGuardados[i] = nivelActual;
                SaveManager.GetIntance().currentSave.levels = nivelesGuardados;
                SaveManager.GetIntance().SaveGame(1);
                return;
            }
        }
        nivelesGuardados.Add(nivelActual);
        SaveManager.GetIntance().currentSave.levels = nivelesGuardados;
        SaveManager.GetIntance().SaveGame(1);
    }
}