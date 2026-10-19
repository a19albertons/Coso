using UnityEngine;


/// <summary>
/// Clase que gestiona el diálogo en la cinemática de introducción
/// </summary>
public class ControllerDialogeIntro : ControllerDialogeBase
{
    private bool[] dialogoMostrado = new bool[5];

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
                localize.StringReference.SetReference(tablaNombre, "DialogoIntro1");
                dialogo.color = Color.blue;
                dialogoMostrado[0] = true;
            }
            else if (time >= 2f && !dialogoMostrado[1])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoIntro2");
                dialogo.color = Color.blue;
                dialogoMostrado[1] = true;
            }
            else if (time >= 5f && !dialogoMostrado[2])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoIntro3");
                dialogo.color = Color.blue;
                dialogoMostrado[2] = true;
            }
            else if (time >= 8f && !dialogoMostrado[3])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoIntro4");
                dialogo.color = Color.red;
                dialogoMostrado[3] = true;
            }
            else if (time >= 11f && !dialogoMostrado[4])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoIntro5");
                dialogo.color = Color.red;
                dialogoMostrado[4] = true;
            }
        }
    }
}