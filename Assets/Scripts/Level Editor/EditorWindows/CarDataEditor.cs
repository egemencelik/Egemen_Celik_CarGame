using UnityEditor;
using UnityEngine;

public class CarDataEditor : EditorWindow
{
    private Object[] carDatas;

    private void OnGUI()
    {
        foreach (var obj in carDatas)
        {
            GUILayout.Space(10);
            var carData = (CarData)obj;
            var speed = carData.speed;
            var turn = carData.turnPower;
            GUILayout.Label(carData.name, EditorStyles.boldLabel);
            carData.speed = EditorGUILayout.FloatField("Speed:", speed);
            carData.turnPower = EditorGUILayout.FloatField("Turn Power:", turn);
            if (GUI.changed)
            {
                EditorUtility.SetDirty(carData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        
    }

    private void Awake()
    {
        carDatas = Resources.LoadAll("Car Data", typeof(CarData));
    }
}