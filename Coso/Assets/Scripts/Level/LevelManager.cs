using System;
using UnityEngine;

/// <summary>
/// Clase LevelManager que gestiona el estado del nivel
public class LevelManager : MonoBehaviour
{


    [SerializeField]
    [Tooltip("Personaje que gestiona el usuario")]
    private GameObject player;


    private EnumStars.Stars stars1;
    private EnumStars.Stars stars2;
    private EnumStars.Stars stars3;

    void Awake()
    {
        // Inicializar las estrellas como nunca obtenidas (provisional)
        stars1 = EnumStars.Stars.NuncaObtenida;
        stars2 = EnumStars.Stars.NuncaObtenida;
        stars3 = EnumStars.Stars.NuncaObtenida;
    }

    /// <summary>
    /// Marca una estrella como obtenida.
    /// </summary>
    /// <param name="estrellaNumero">Numero de estrella del 1 al 3</param>
    public void EstrellaObtenida(int estrellaNumero)
    {
        if (estrellaNumero == 1)
        {
            stars1 = EnumStars.Stars.Obtenida;
        }
        else if (estrellaNumero == 2)
        {
            stars2 = EnumStars.Stars.Obtenida;
        }
        else if (estrellaNumero == 3)
        {
            stars3 = EnumStars.Stars.Obtenida;
        }
    }

    /// <summary>
    /// Obtiene la información de todas las estrellas del nivel.
    /// </summary>
    /// <returns>Devuelve un enum con el estado de las estrellas del nivel.</returns>
    public EnumStars.Stars[] InformaciónTodasLasEstrellas()
    {

        return new EnumStars.Stars[] { stars1, stars2, stars3 };
    }


}