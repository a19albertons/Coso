using UnityEngine;

/// <summary>
/// Añade funciones matemáticas adicionales a las que ofrece unity por defecto
/// </summary>
public static class MathAdditional
{
    private static readonly float EXPONENTE_CURVA = 1.75f; // Exponente para suavizar la curva de volumen

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
    /// Mantenido por deepseek v4.1 flash y gemini 4 hydrogen
    /// </summary>
    /// <param name="valorSlider">valor del slider del 0-100</param>
    /// <returns>devuelve los decibelios</returns>
    public static float SetVolume(float valorSlider) // valorSlider va de 0 a 100
    {
        // 1. Convertimos de 0-100 a 0-1
        float valorNormalizado = valorSlider / 100f;

        // 2. Suavizamos la curva para repartir mejor los decibelios (evita el salto al final)
        // Con exponente 1.75f: 50% = -10.5 dB, 10% = -35 dB, 1% = -70 dB, 0% = -80 dB
        float valorCurvado = Mathf.Pow(valorNormalizado, EXPONENTE_CURVA);

        // 3. Aplicamos el Clamp DESPUÉS de la potencia para no bajar de -80 dB
        valorCurvado = Mathf.Clamp(valorCurvado, 0.0001f, 1f);

        // 4. Conversión a decibelios
        return Mathf.Log10(valorCurvado) * 20f;
    }

    /// <summary>
    /// Recibe un valor en decibelios de un mixer y lo convierta a un valor del 0 al 100 para el slider.
    /// Mantenida por deepseek v4.1 flash y gemini 4 hydrogen
    /// </summary>
    /// <param name="decibelios">decibelios del mixer</param>
    /// <returns>devuelve el valor para el slider del 1 al 100</returns>
    public static float GetVolumeForSlider(float decibelios) // -80 a 0 dB -> 0 a 100
    {
        // Si está en el silencio mínimo del mixer, devolvemos 0 exacto
        if (decibelios <= -80f)
            return 0f;

        // 1. Invertimos el logaritmo: valorCurvado = 10 ^ (dB / 20)
        float valorCurvado = Mathf.Pow(10f, decibelios / 20f);

        // 2. Invertimos la potencia (raíz 1 / 1.75)
        float valorNormalizado = Mathf.Pow(valorCurvado, 1f / EXPONENTE_CURVA);

        // 3. Pasamos a escala 0-100
        return Mathf.Clamp(valorNormalizado * 100f, 0f, 100f);
    }
}
