using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CarPickerEditor : EditorWindow
{
    private Object[] carDatas;

    private void OnGUI()
    {
        foreach (var obj in carDatas)
        {
            var carData = (CarData)obj;
            
            GUILayout.Space(10);
            
            if (GUILayout.Button(carData.name))
            {
                LevelEditorController.CarCount++;
                LevelEditorController.Status = LevelEditorController.EditorStatus.PlacingCar;
                var carGO = Instantiate(Resources.Load<GameObject>($"Prefabs/{carData.name}"), GameObject.Find("Cars").transform);
                carGO.name = "Car " + LevelEditorController.CarCount;
                carGO.GetComponent<LevelEditorObject>().SetPositionAfterFrame();
                Selection.activeObject = carGO;
                EditorSceneManager.MarkAllScenesDirty();
                EditorSceneManager.SaveOpenScenes();
                Close();
            }
        }
    }

    private void Awake()
    {
        carDatas = Resources.LoadAll("Car Data", typeof(CarData));
    }
}