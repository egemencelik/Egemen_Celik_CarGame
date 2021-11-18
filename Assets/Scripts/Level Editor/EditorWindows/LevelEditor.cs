using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEngine;
using static System.String;

#if UNITY_EDITOR

public class LevelEditor : EditorWindow
{
    [MenuItem("Custom Tools/Level Editor")]
    public static void ShowEditor()
    {
        LevelEditorController.Status = LevelEditorController.EditorStatus.Empty;
        GetWindow(typeof(LevelEditor), false, "Level Editor");
    }

    private static GameObject SelectedTarget;
    private static GameObject SelectedStart;

    private void OnSelectionChange()
    {
        Repaint();

        if (Selection.activeObject == null)
        {
            LevelEditorController.Status = LevelEditorController.EditorStatus.Empty;
            SelectedStart = null;
            return;
        }

        if (LevelEditorController.Status == LevelEditorController.EditorStatus.ConnectingStartAndTarget)
        {
            if (Selection.activeObject.name.StartsWith("Target"))
            {
                SelectedTarget = (GameObject)Selection.activeObject;
                SwapDrawLines(SelectedStart, SelectedTarget);
            }

            LevelEditorController.Status = LevelEditorController.EditorStatus.Empty;
            SelectedStart = null;
            SelectedTarget = null;
        }
    }

    private static void SwapDrawLines(GameObject start, GameObject target)
    {
        var newNumber = start.name.Split(' ')[1];
        var oldTarget = GameObject.Find("Target " + newNumber);

        oldTarget.name = "temp";
        var oldName = target.name;
        target.name = "Target " + newNumber;
        oldTarget.name = oldName;
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Objects", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Car"))
        {
            GetWindow(typeof(CarPickerEditor), true, "Car Picker");
        }

        if (GUILayout.Button("Create Obstacle"))
        {
            LevelEditorController.Status = LevelEditorController.EditorStatus.PlacingObstacle;
            var obstacle = Instantiate(Resources.Load<GameObject>("Prefabs/Obstacle"), GameObject.Find("Obstacles").transform);
            obstacle.GetComponent<LevelEditorObject>().SetPositionAfterFrame();
            obstacle.transform.SetPosZ(6);
            Selection.activeObject = obstacle;
            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
        }

        GUILayout.Space(20);
        GUILayout.Label("Utility", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Create Empty Level"))
        {
            SceneTemplateService.Instantiate(Resources.Load<SceneTemplateAsset>("Scene Templates/Template"), false);
            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
        }

        if (GUILayout.Button("Clear Level"))
        {
            LevelEditorController.Status = LevelEditorController.EditorStatus.Empty;
            ClearLevel();
        }

        if (GUILayout.Button("Reorder Objects"))
        {
            ReorderObjects();
        }

        if (GUILayout.Button("Change Car Data"))
        {
            GetWindow(typeof(CarDataEditor), true, "Car Data Editor");
        }

        GUI.enabled = Selection.activeObject != null && Selection.activeObject.name.StartsWith("Start");

        if (GUILayout.Button("Set Target"))
        {
            LevelEditorController.Status = LevelEditorController.EditorStatus.ConnectingStartAndTarget;
            SelectedStart = Selection.activeGameObject;
        }

        GUI.enabled = true;

        GUILayout.Space(20);
        GUILayout.Label("Import/Export", EditorStyles.boldLabel);

        if (GUILayout.Button("Save Level"))
        {
            var json = JsonHelper.CreateLevelJSON();

            var path = EditorUtility.SaveFilePanel(
                "Save current level as JSON",
                "",
                "level.json",
                "json");

            if (path.Length != 0)
            {
                if (!IsNullOrEmpty(json))
                    File.WriteAllText(path, json);
            }
        }

        if (GUILayout.Button("Load Level"))
        {
            string path = EditorUtility.OpenFilePanel("Load level from JSON", "", "json");
            if (path.Length != 0)
            {
                var json = File.ReadAllText(path);
                ClearLevel();
                JsonHelper.LoadLevelFromJSON(json);
            }
        }
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("How to Use"))
        {
            GetWindow(typeof(HelpEditor), true, "How to Use");
        }
    }

    private static void ReorderObjects()
    {
        Reorder(GameObject.Find("StartPoints").transform);
        Reorder(GameObject.Find("TargetPoints").transform);
        Reorder(GameObject.Find("Cars").transform);
    }

    private static void Reorder(Transform transform)
    {
        var list = (from Transform tr in transform select tr.name).ToList();
        list.Sort((x, y) => Compare(x, y, StringComparison.Ordinal));

        foreach (Transform tr in transform)
        {
            tr.SetSiblingIndex(list.IndexOf(tr.name));
        }
    }

    private static void ClearLevel()
    {
        DeleteChildren(GameObject.Find("StartPoints"));
        DeleteChildren(GameObject.Find("TargetPoints"));
        DeleteChildren(GameObject.Find("Cars"));
        DeleteChildren(GameObject.Find("Obstacles"));
        LevelEditorController.CarCount = 0;
        EditorSceneManager.MarkAllScenesDirty();
        EditorSceneManager.SaveOpenScenes();
    }

    private static void DeleteChildren(GameObject go)
    {
        while (go.transform.childCount > 0)
            foreach (Transform child in go.transform)
            {
                DestroyImmediate(child.gameObject);
            }
    }
}

#endif