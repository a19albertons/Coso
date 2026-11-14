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
    private int numeroSlot;

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

    public static SaveManager GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Guarda el estado actual del current save en el slot que internamente este indicado
    /// </summary>
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
        // Guardamos el slot para futuros usos
        numeroSlot = slot;

        // Busca el fichero si no lo encuentra considera que nunca existio
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
    public void NewGame(int slot)
    {
        // Guardamos el slot para futuros usos
        numeroSlot = slot;

        // Crea un nuevo save
        currentSave = new CurrentSave();
    }
}