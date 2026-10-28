
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
    /// Debe invocarse por linea de comando un argumento de compresion, por ejemplo: -compressionFormat=Gzip o -compressionFormat=Brotli. Cualquier otro valor usará el de por defecto de unity.
    /// Debe invocarse por linea de comando un argumento de ruta de salida, por ejemplo: -outputPath=Builds/WebGL. Cualquier otro valor usará el de por defecto de unity.
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
        string outputPath = "";

        foreach (string arg in args)
        {
            if (arg.StartsWith("-compressionFormat="))
            {
                argumentoCompresion = arg.Split('=')[1];
            }

            if (arg.StartsWith("-outputPath="))
            {
                outputPath = arg.Split('=')[1];
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


        BuildPipeline.BuildPlayer(buildPlayerOptions);

        // Restaura valores originales de configuracion de unity
        PlayerSettings.WebGL.compressionFormat = formatoOriginal;
        AssetDatabase.SaveAssets();
    }
}