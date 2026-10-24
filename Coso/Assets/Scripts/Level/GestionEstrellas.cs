using System;
using UnityEngine;

/// <summary>
/// Clase GestionEstrellas que gestiona el estado de las estrellas en el nivel.
/// </summary>
public class GestionEstrellas : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Estrellas del nivel. En orden correcta")]
    private GameObject[] estrellas;

    private EnumStateLevelYStars.LevelYStars stars1;
    private EnumStateLevelYStars.LevelYStars stars2;
    private EnumStateLevelYStars.LevelYStars stars3;

    void Awake()
    {
        if (SaveManager.GetInstance().currentSave == null)
        {
            Debug.LogError("No se ha cargado ningún archivo de guardado. Asegúrate de que el SaveManager esté inicializado correctamente.");
            return;
        }
        SaveManager.GetInstance().currentSave.levels.ForEach(level =>
        {
    
            if (level.identificadorNivel.Equals(gameObject.scene.name))
            {
                stars1 = level.starsObtained[0];
                stars2 = level.starsObtained[1];
                stars3 = level.starsObtained[2];
            }
        });

        estrellas[0].SetActive(stars1 == EnumStateLevelYStars.LevelYStars.NoObtenida);
        estrellas[1].SetActive(stars2 == EnumStateLevelYStars.LevelYStars.NoObtenida);
        estrellas[2].SetActive(stars3 == EnumStateLevelYStars.LevelYStars.NoObtenida);
    }

    /// <summary>
    /// Marca una estrella como obtenida.
    /// </summary>
    /// <param name="estrellaNumero">Numero de estrella del 1 al 3</param>
    public void EstrellaObtenida(int estrellaNumero)
    {
        if (estrellaNumero == 1)
        {
            stars1 = EnumStateLevelYStars.LevelYStars.Provisional;
        }
        else if (estrellaNumero == 2)
        {
            stars2 = EnumStateLevelYStars.LevelYStars.Provisional;
        }
        else if (estrellaNumero == 3)
        {
            stars3 = EnumStateLevelYStars.LevelYStars.Provisional;
        }
    }

    /// <summary>
    /// Obtiene la información de todas las estrellas del nivel.
    /// </summary>
    /// <returns>Devuelve un enum con el estado de las estrellas del nivel.</returns>
    public EnumStateLevelYStars.LevelYStars[] InformaciónTodasLasEstrellas()
    {

        return new EnumStateLevelYStars.LevelYStars[] { stars1, stars2, stars3 };
    }


}