
using System;
using System.Linq;
using UnityEditor;


/// <summary>
/// Script que gestiona metodos personalizados de compilación de la aplicación
/// </summary>
public class BuildScript
{
    /// <summary>
    /// Metodo a invocar para gestionar la compilación personalizda de webGL
    /// Debe invocarse por linea de comando un argumento de compresion, por ejemplo: -compressionFormat Gzip o -compressionFormat Brotli. Cualquier otro valor usará el de por defecto de unity.
    /// Debe invocarse por linea de comando un argumento de ruta de salida, por ejemplo: -customBuildPath Builds/WebGL. Cualquier otro valor usará el de por defecto de unity.
    /// </summary>
    public static void BuildWebGL()
    {
        // Variables iniciales
        String[] args = Environment.GetCommandLineArgs();
        string argumentoCompresion = "";
        var formatoOriginal = PlayerSettings.WebGL.compressionFormat;
        var escenasHabilitadas = EditorBuildSettings.scenes
        .Where(scene => scene.enabled)
        .Select(scene => scene.path)
        .ToArray();
        string outputPath = "Builds/WebGL";

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (String.Equals(arg, "-compressionFormat", StringComparison.Ordinal))
            {
                i++;
                argumentoCompresion = args[i];
            }

            if (String.Equals(arg, "-customBuildPath", StringComparison.Ordinal))
            {
                i++;
                outputPath = args[i].Trim();
            }
        }

        switch (argumentoCompresion)
        {
            case "Gzip":
                PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
                break;
            case "Brotli":
                PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
                break;
            default:
                break;
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = escenasHabilitadas;

        buildPlayerOptions.locationPathName = outputPath;
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;

        // Añadimos que en caso de error en esta operación lance una excepción y saber la causa.
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        // Restaura valores originales de configuracion de unity
        PlayerSettings.WebGL.compressionFormat = formatoOriginal;
        AssetDatabase.SaveAssets();

        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            throw new Exception($"Unity Build failed with result: {report.summary.result}");
        }
    }
}