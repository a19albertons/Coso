using UnityEngine;

/// <summary>
/// Clase que gestiona el diálogo del final bueno
/// </summary>
public class ControllerDialogeFinalBueno : ControllerDialogeBase
{
    private bool[] dialogoMostrado = new bool[7];

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
                dialogo.text = "Sí, pero mejor avanzo dos bloques.";
                dialogo.color = Color.blue;
                dialogoMostrado[3] = true;
            }
            else if (time >= 11f && !dialogoMostrado[4])
            {
                dialogo.text = "He visto la roca";
                dialogo.color = Color.blue;
                dialogoMostrado[4] = true;
            }
            else if (time >= 12f && !dialogoMostrado[5])
            {
                dialogo.text = "Veo que has aprendido y no has tenido que repetir ninguna prueba.";
                dialogo.color = Color.red;
                dialogoMostrado[5] = true;
            }
            else if (time >= 15f)
            {
                dialogo.text = "Vamos en esta dirreción para haablar de todos los preparativos de la boda.";
                dialogo.color = Color.red;
                dialogoMostrado[6] = true;
            }
        }
    }
}