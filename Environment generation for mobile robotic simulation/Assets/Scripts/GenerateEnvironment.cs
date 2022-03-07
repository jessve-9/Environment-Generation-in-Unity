using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

class GenerateEnvironment : MonoBehaviour
{
    static void StartGeneration ()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/autoScene.unity");
        string[] args = System.Environment.GetCommandLineArgs();
        int amountOfArgs = 7;
        float heightOffsetElevated = 13f;
        float tiltZ = 0f;
        float tiltX = 0f;
        bool useFalloff = true;
        for(int i = 0; i < args.Length; i++){
            Debug.Log(args[i]);
            if(string.Equals(args[i], "-heightOffsetElevated")){
                heightOffsetElevated = float.Parse(args[i+1]);
            }
            else if(string.Equals(args[i], "-tiltZ")){
                tiltZ = float.Parse(args[i+1]);
            }
            else if(string.Equals(args[i], "-tiltX")){
                tiltX = float.Parse(args[i+1]);
            }
            else if(string.Equals(args[i], "-useFalloff")){
                useFalloff = bool.Parse(args[i+1]);
            }
        }
        // float heightOffsetElevated = float.Parse(args[amountOfArgs]);
        // float tiltZ = float.Parse(args[amountOfArgs+1]);
        // float tiltX = float.Parse(args[amountOfArgs+2]);
        // bool useFalloff = bool.Parse(args[amountOfArgs+3]);
        MapGeneratorAuto mapGenerator = new MapGeneratorAuto();
        mapGenerator.RandomizeOnMapType(heightOffsetElevated, tiltZ, tiltX, useFalloff);
        OBJExporter objExporter = new OBJExporter();
        objExporter.ExportAuto();   //Will export to the most recent folder that you exported to
    }
}