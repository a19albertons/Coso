using System.IO;
using TMPro;
using UnityEngine;

/// <summary>
/// CargarEstadoSlot carga el estado del slot de guardado y actualiza la UI con los datos del jugador y niveles.
/// </summary>
[RequireComponent(typeof(ControladorSlot))]
public class CargarEstadoSlot : MonoBehaviour
{
    int numeroSlot;

    [SerializeField]
    private TextMeshProUGUI contadorVidas;

    [SerializeField]
    private TextMeshProUGUI contadorMonedas;

    [SerializeField]
    private TextMeshProUGUI contadorEstrellas;

    [SerializeField]
    private int maximoEstrellas = 24;

    [SerializeField]
    private TextMeshProUGUI nivelActual;

    void Start()
    {
        numeroSlot = GetComponent<ControladorSlot>().numeroSlot;
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "slotGuardado" + numeroSlot));
        CurrentSave procesar = JsonUtility.FromJson<CurrentSave>(json);

        GestiionarVariablesJugador(procesar);
        GestionEstrellaYNivelActual(procesar);
    }

    /// <summary>
    /// Gestiona las variables cde jugador como vidas y monedas a partir del estado guardado en el slot.
    /// </summary>
    /// <param name="procesar"></param>
    private void GestiionarVariablesJugador(CurrentSave procesar)
    {
        // Asignar variables de jugador
        contadorVidas.text =  procesar.playerInfo.vidasActuales.ToString();
        contadorMonedas.text = procesar.playerInfo.monedasActuales.ToString();
    }

    /// <summary>
    /// Gestiona el recuento de estrellas y el nivel actual a partir del estado guardado en el slot.
    /// </summary>
    /// <param name="procesar"></param>
    private void GestionEstrellaYNivelActual(CurrentSave procesar)
    {
                int recuentoEstrellas = 0;
        int contador  = 1;
        // Asignar variable de estrellas leyendo los niveles existentes
        for (int i = 0; i < procesar.levels.Count; i++)
        {
            // Caclculo estrellas
            foreach (EnumStateLevelYStars.LevelYStars item in procesar.levels[i].starsObtained)
            {
                if (item == EnumStateLevelYStars.LevelYStars.Obtenida || item == EnumStateLevelYStars.LevelYStars.Provisional)
                {
                    recuentoEstrellas++;
                }
            }

            contador++;
        }
        contadorEstrellas.text = recuentoEstrellas.ToString()+"/"+maximoEstrellas.ToString();
        // Controla que la definición de nivel no exceda el 1-8 que es el ultimo
        if (contador > 8)
        {
            nivelActual.text = "1-8";
        }
        else
        {
            nivelActual.text = "1-"+contador;
        }
    }
}