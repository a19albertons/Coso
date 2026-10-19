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
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalBueno1");
                dialogo.color = Color.blue;
                dialogoMostrado[0] = true;
            }
            else if (time >= 3f && !dialogoMostrado[1])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalBueno2");
                dialogo.color = Color.red;
                dialogoMostrado[1] = true;
            }
            else if (time >= 7f && !dialogoMostrado[2])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalBueno3");
                dialogo.color = Color.red;
                dialogoMostrado[2] = true;
            }
            else if (time >= 9f && !dialogoMostrado[3])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalBueno4");
                dialogo.color = Color.blue;
                dialogoMostrado[3] = true;
            }
            else if (time >= 11f && !dialogoMostrado[4])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalBueno5");
                dialogo.color = Color.blue;
                dialogoMostrado[4] = true;
            }
            else if (time >= 12f && !dialogoMostrado[5])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalBueno6");
                dialogo.color = Color.red;
                dialogoMostrado[5] = true;
            }
            else if (time >= 15f)
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalBueno7");
                dialogo.color = Color.red;
                dialogoMostrado[6] = true;
            }
        }
    }
}