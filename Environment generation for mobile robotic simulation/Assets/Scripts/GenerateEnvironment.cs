using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

class GenerateEnvironment : MonoBehaviour
{
    static void StartGeneration ()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/autoScene.unity");
        string[] args = System.Environment.GetCommandLineArgs();
        for(int i = 7; i < args.Length; i++){
            //Debug.Log(args[i]);
        }
        float heightOffsetElevated = float.Parse(args[7]);
        float tiltZ = float.Parse(args[8]);
        float tiltX = float.Parse(args[9]);
        bool useFalloff = bool.Parse(args[10]);
        MapGeneratorAuto mapGenerator = new MapGeneratorAuto();
        mapGenerator.RandomizeOnMapType(heightOffsetElevated, tiltZ, tiltX, useFalloff);
        OBJExporter objExporter = new OBJExporter();
        objExporter.ExportAuto();   //Will export to the most recent folder that you exported to
    }
}