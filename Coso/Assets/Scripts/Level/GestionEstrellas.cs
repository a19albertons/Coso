using System;
using UnityEngine;

/// <summary>
/// Clase GestionEstrellas que gestiona el estado de las estrellas en el nivel.
/// </summary>
public class GestionEstrellas : MonoBehaviour
{

    private EnumStateLevelYStars.LevelYStars stars1;
    private EnumStateLevelYStars.LevelYStars stars2;
    private EnumStateLevelYStars.LevelYStars stars3;

    void Awake()
    {
        // Inicializar las estrellas como nunca obtenidas (provisional)
        stars1 = EnumStateLevelYStars.LevelYStars.NoObtenida;
        stars2 = EnumStateLevelYStars.LevelYStars.NoObtenida;
        stars3 = EnumStateLevelYStars.LevelYStars.NoObtenida;
    }

    /// <summary>
    /// Marca una estrella como obtenida.
    /// </summary>
    /// <param name="estrellaNumero">Numero de estrella del 1 al 3</param>
    public void EstrellaObtenida(int estrellaNumero)
    {
        if (estrellaNumero == 1)
        {
            stars1 = EnumStateLevelYStars.LevelYStars.Obtenida;
        }
        else if (estrellaNumero == 2)
        {
            stars2 = EnumStateLevelYStars.LevelYStars.Obtenida;
        }
        else if (estrellaNumero == 3)
        {
            stars3 = EnumStateLevelYStars.LevelYStars.Obtenida;
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