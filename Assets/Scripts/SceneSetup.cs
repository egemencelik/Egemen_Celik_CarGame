#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class SceneSetup
{
    private static string currentScene;

    static SceneSetup()
    {
        currentScene = SceneManager.GetActiveScene().name;
        EditorApplication.hierarchyChanged += HierarchyWindowChanged;
    }

    private static void HierarchyWindowChanged()
    {
        if (currentScene != SceneManager.GetActiveScene().name)
        {
            //a scene change has happened
            currentScene = SceneManager.GetActiveScene().name;
            var objs = Object.FindObjectsOfType<LevelEditorObject>();
            foreach (var levelEditorObject in objs)
            {
                levelEditorObject.positionSet = true;
            }
        }
    }
}

#endif