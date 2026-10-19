using UnityEngine;

/// <summary>
/// Clase que gestiona el diálogo en la cinemática del final malo
/// </summary>
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
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalMalo1");
                dialogo.color = Color.blue;
                dialogoMostrado[0] = true;
            }
            else if (time >= 3f && !dialogoMostrado[1])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalMalo2");
                dialogo.color = Color.red;
                dialogoMostrado[1] = true;
            }
            else if (time >= 7f && !dialogoMostrado[2])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalMalo3");
                dialogo.color = Color.red;
                dialogoMostrado[2] = true;
            }
            else if (time >= 9f && !dialogoMostrado[3])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalMalo4");
                dialogo.color = Color.blue;
                dialogoMostrado[3] = true;
            }
            else if (time >= 10f && !dialogoMostrado[4])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalMalo5");
                dialogo.color = Color.red;
                dialogoMostrado[4] = true;
            }
            else if (time >= 11f && !dialogoMostrado[5])
            {
                localize.StringReference.SetReference(tablaNombre, "DialogoFinalMalo6");
                dialogo.color = Color.red;
                dialogoMostrado[5] = true;
            }
        }
    }
}