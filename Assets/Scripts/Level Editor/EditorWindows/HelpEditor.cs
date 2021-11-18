using UnityEditor;
using UnityEngine;

public class HelpEditor : EditorWindow
{
    private void OnGUI()
    {
        GUILayout.Label("Create Car", EditorStyles.boldLabel);
        GUILayout.Label(
            "New window pops up and shows you the cars you can create. After you select a car it automatically follows your mouse until you click your left mouse button and set its position. When you set position new start and target points will be instantiated at your cars position. The target point will follow your mouse until you click again and set its position.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        GUILayout.Label("Create Obstacle", EditorStyles.boldLabel);
        GUILayout.Label(
            "Instantiates new obstacle and it automatically follows your mouse until you click your left mouse button and set its position.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);
        
        GUILayout.Label("Create Empty Level", EditorStyles.boldLabel);
        GUILayout.Label(
            "Creates a new scene from template.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        GUILayout.Label("Clear Level", EditorStyles.boldLabel);
        GUILayout.Label(
            "Deletes all children under Cars, StartPoints, TargetPoints and Obstacles, then saves scene.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        GUILayout.Label("Reorder Objects", EditorStyles.boldLabel);
        GUILayout.Label(
            "Reorders all children under Cars, StartPoints and TargetPoints by their number in their names.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        GUILayout.Label("Change Car Data", EditorStyles.boldLabel);
        GUILayout.Label(
            "New window pops up and shows you the car data you can edit. When you edit a field it automatically edits the scriptable object as well.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        GUILayout.Label("Set Target", EditorStyles.boldLabel);
        GUILayout.Label(
            "Becomes enabled if select a Start Point. After you click the Set Target button, you should select a Target Point object to connect them.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        GUILayout.Label("Save Level", EditorStyles.boldLabel);
        GUILayout.Label(
            "Save current level as a json file.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);

        GUILayout.Label("Load Level", EditorStyles.boldLabel);
        GUILayout.Label(
            "Load level from a json file.",
            EditorStyles.wordWrappedLabel);
        GUILayout.Space(10);
    }
}