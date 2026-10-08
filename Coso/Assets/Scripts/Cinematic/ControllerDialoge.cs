using TMPro;
using UnityEngine;

public class ControllerDialoge : MonoBehaviour
{
    private float time;
    private bool tiempoIniciado = false;
    private bool[] dialogoMostrado = new bool[5];

    private TextMeshProUGUI dialogo;

    void Awake()
    {
        dialogo = GetComponentInChildren<TextMeshProUGUI>();
        for (int i = 0; i < dialogoMostrado.Length; i++)
        {
            dialogoMostrado[i] = false;
        }
    }
    public void IniciarTiempo()
    {
        time = 0f;
        tiempoIniciado = true;
    }

    void Update()
    {
        if (tiempoIniciado)
        {
            time += Time.deltaTime;

            if (time >= 0f && !dialogoMostrado[0])
            {
                dialogo.text = "Hola, Cosa";
                dialogo.color = Color.blue;
                dialogoMostrado[0] = true;
            }
            else if (time >= 2f && !dialogoMostrado[1])
            {
                dialogo.text = "Llevamos mucho tiempo juntos como novios. No sé como decirlo...";
                dialogo.color = Color.blue;
                dialogoMostrado[1] = true;
            }
            else if (time >= 5f && !dialogoMostrado[2])
            {
                dialogo.text = "¿Quieres CASARTE CONMIGO?";
                dialogo.color = Color.blue;
                dialogoMostrado[2] = true;
            }
            else if (time >= 8f && !dialogoMostrado[3])
            {
                dialogo.text = "Sí, pero antes tienes que pasar una serie de pruebas.";
                dialogo.color = Color.red;
                dialogoMostrado[3] = true;
            }
            else if (time >= 11f && !dialogoMostrado[4])
            {
                dialogo.text = "Si de verdad quieres casarte conmigo, sigueme. Las pruebas te estan esperando";
                dialogo.color = Color.red;
                dialogoMostrado[4] = true;
            }
        }
    }
}