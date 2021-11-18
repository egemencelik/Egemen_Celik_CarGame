#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(DrawLines))]
public class DrawLinesEditor : Editor
{
    private DrawLines origin;
    private GameObject startGO, targetGO;

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;
        OnSelectionChanged();
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage()) return; // Don't search for objects if in prefab mode

        origin = (DrawLines)target;

        var number = origin.name.Split(' ')[1];
        startGO = GameObject.Find("Start " + number);
        targetGO = GameObject.Find("Target " + number);
    }

    private void OnSceneGUI()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage()) return; // Don't update GUI if in prefab mode

        origin = (DrawLines)target;
        Handles.color = Color.yellow;
        
        EditorGUI.BeginChangeCheck();
        Quaternion rot = Handles.RotationHandle(origin.rot, origin.transform.position);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Rotated RotateAt Point");
            origin.rot = rot;
            origin.Update();
        }

        if (!startGO || !targetGO) return; // Don't draw if can't find start and target

        Handles.DrawDottedLine(startGO.transform.position, targetGO.transform.position, 8);
        Handles.Label(origin.transform.position + new Vector3(0, .6f, 0), origin.name);
    }
}

#endif