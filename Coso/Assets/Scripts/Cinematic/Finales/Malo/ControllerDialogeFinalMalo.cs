using TMPro;
using UnityEngine;

public class ControllerDialogeFinalMalo : ControllerDialogeBase
{
    private bool[] dialogoMostrado = new bool[6];

    protected override void InicializarDialogoMostrado()
    {
        for (int i = 0; i < dialogoMostrado.Length; i++)
        {
            dialogoMostrado[i] = false;
        }
    }


    protected override void Dialogo()
    {
        if (tiempoIniciado)
        {
            time += Time.deltaTime;

            if (time >= 0f && !dialogoMostrado[0])
            {
                dialogo.text = "Hola, Cosa. ¿Qué tal estás?";
                dialogo.color = Color.blue;
                dialogoMostrado[0] = true;
            }
            else if (time >= 3f && !dialogoMostrado[1])
            {
                dialogo.text = "Yo muy bien veo que has terminado las pruebas.";
                dialogo.color = Color.red;
                dialogoMostrado[1] = true;
            }
            else if (time >= 7f && !dialogoMostrado[2])
            {
                dialogo.text = "¿Puedes avanzar un bloque?";
                dialogo.color = Color.red;
                dialogoMostrado[2] = true;
            }
            else if (time >= 9f && !dialogoMostrado[3])
            {
                dialogo.text = "Sí";
                dialogo.color = Color.blue;
                dialogoMostrado[3] = true;
            }
            else if (time >= 10f && !dialogoMostrado[4])
            {
                dialogo.text = "Espera ahí un momento.";
                dialogo.color = Color.red;
                dialogoMostrado[4] = true;
            }
            else if (time >= 11f && !dialogoMostrado[5])
            {
                dialogo.text = "Perfecto, habías tenido que repetir algunas pruebas";
                dialogo.color = Color.red;
                dialogoMostrado[5] = true;
            }
        }
    }
}