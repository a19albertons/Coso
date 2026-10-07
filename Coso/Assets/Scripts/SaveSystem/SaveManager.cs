using System.IO;
using UnityEngine;

public class SaveManager
{
    private static SaveManager instance;

    public CurrentSave currentSave;
    private string rutaBase = Path.Combine(Application.persistentDataPath,"slotGuardado");

    public static SaveManager GetIntance()
    {
        if (instance == null)
        {
            instance = new SaveManager();
        }
        return instance;
    }

    /// <summary>
    /// Metodo que guarda el juego con soporte para slots
    /// </summary>
    /// <param name="slot">El numero de slot</param>
    public void SaveGame(int slot)
    {
        string guardado = JsonUtility.ToJson(currentSave);
        File.WriteAllText(rutaBase+slot, guardado);
    }

    /// <summary>
    /// Metodo que carga el juego del slot indicado
    /// </summary>
    /// <param name="slot"></param>
    public void LoadGame(int slot)
    {
        if (File.Exists(rutaBase + slot))
        {
            string guardado = File.ReadAllText(rutaBase);
            currentSave = JsonUtility.FromJson<CurrentSave>(guardado);
            return;
        }
        currentSave=null;
    }

    public void NewGame()
    {
        currentSave = new CurrentSave();
    }
}