using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GestionSlider : MonoBehaviour
{
    private Slider progreso;

    void Awake()
    {
        progreso = GetComponent<Slider>();
    }

    void Update()
    {
        if (GestionEscenas.GetInstance().opStatus != null)
        {
            progreso.value = (float)(GestionEscenas.GetInstance().opStatus.progress/0.92);
        }
        else
        {
            progreso.value = 0;
        }
    }
}