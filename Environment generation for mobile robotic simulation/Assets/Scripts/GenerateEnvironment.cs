using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
class GenerateEnvironment : MonoBehaviour
{
    static void StartGeneration ()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/autoScene.unity");
        MapGeneratorAuto mapGenerator = new MapGeneratorAuto();
        mapGenerator.RandomizeOnMapType();
        OBJExporter objExporter = new OBJExporter();
        objExporter.ExportAuto();   //Will export to the most recent folder that you exported to
    }
}