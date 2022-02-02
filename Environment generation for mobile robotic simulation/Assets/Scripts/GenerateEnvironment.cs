using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
class GenerateEnvironment : MonoBehaviour
{
    static void StartGeneration ()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/scene.unity");
        MapGenerator mapGenerator = new MapGenerator();
        mapGenerator.GenerateMap();
    }
}