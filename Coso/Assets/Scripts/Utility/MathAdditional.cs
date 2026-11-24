using UnityEngine;

/// <summary>
/// Añade funciones matemáticas adicionales a las que ofrece unity por defecto
/// </summary>
public static class MathAdditional
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

    /// <summary>
    /// Recibe un valor del slidder y lo convierte a decibelios
    /// Mantenido por deepseek v4.1 flash
    /// </summary>
    /// <param name="valorSlider">valor del slider del 0-100</param>
    /// <returns>devuelve los decibelios</returns>
    public static float SetVolume(float valorSlider) // valorSlider va de 0 a 100
    {
        // 1. Convertimos el valor de 0-100 a 0-1
        float valorNormalizado = valorSlider / 100f;

        // 2. Evitamos el logaritmo de 0 (que da infinito negativo)
        // Usamos 0.0001f para representar el silencio absoluto, que en Unity es -80 dB
        valorNormalizado = Mathf.Clamp(valorNormalizado, 0.0001f, 1f);

        // 3. Fórmula matemática: dB = Log10(valor) * 20
        float decibelios = Mathf.Log10(valorNormalizado) * 20f;

        return decibelios;
    }

    /// <summary>
    /// Recibe un valor en decibelios de un mixer y lo convierta a un valor del 0 al 100 para el slider.
    /// Mantenida por deepseek v4.1 flash
    /// </summary>
    /// <param name="decibelios">decibelios del mixer</param>
    /// <returns>devuelve el valor para el slider del 1 al 100</returns>
    public static float GetVolumeForSlider(float decibelios)
    {
        // Fórmula inversa: Amplitud = 10 ^ (dB / 20)
        float amplitud = Mathf.Pow(10f, decibelios / 20f);

        // Lo pasamos a escala 0-100
        float valorSlider = amplitud * 100f;

        // Lo limitamos por si acaso
        return Mathf.Clamp(valorSlider, 0f, 100f);
    }
}