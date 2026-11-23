using UnityEngine;

/// <summary>
/// Añade funciones matemáticas adicionales a las que ofrece unity por defecto
/// </summary>
public class MathAdditional
{
    /// <summary>
    /// Obtiene de 2 valores su maximo comun divisor.
    /// Mantenida por deepseek v4.1 flash
    /// </summary>
    /// <param name="a">Valor a</param>
    /// <param name="b">Valor b</param>
    /// <returns>devuelve el valor</returns>
    public static int GetGCD(int a, int b)
    {
        a = Mathf.Abs(a);
        b = Mathf.Abs(b);

        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}