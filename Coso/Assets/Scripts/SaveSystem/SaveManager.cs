using System.IO;
using UnityEngine;

/// <summary>
/// clase que gestiona el guardado y carga del juego, permitiendo guardar y cargar la partida actual en diferentes slots de guardado.
/// </summary>
public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public CurrentSave currentSave;
    private string rutaBase;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            rutaBase = Path.Combine(Application.persistentDataPath, "slotGuardado");
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static SaveManager GetIntance()
    {
        return instance;
    }

    /// <summary>
    /// Metodo que guarda el juego con soporte para slots
    /// </summary>
    /// <param name="slot">El numero de slot</param>
    public void SaveGame(int slot)
    {
        string guardado = JsonUtility.ToJson(currentSave);
        File.WriteAllText(rutaBase + slot, guardado);
    }

    /// <summary>
    /// Metodo que carga el juego del slot indicado
    /// </summary>
    /// <param name="slot"></param>
    public void LoadGame(int slot)
    {
        if (File.Exists(rutaBase + slot))
        {
            string guardado = File.ReadAllText(rutaBase + slot);
            currentSave = JsonUtility.FromJson<CurrentSave>(guardado);
            return;
        }
        currentSave = null;
    }

    /// <summary>
    /// Metodo que crea un nuevo juego, inicializando el CurrentSave
    /// </summary>
    public void NewGame()
    {
        currentSave = new CurrentSave();
    }
}