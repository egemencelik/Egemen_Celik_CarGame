#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
public class LevelEditorCar : LevelEditorObject
{
    protected override void OnMouseClick()
    {
        base.OnMouseClick();

        var startPoint = Instantiate(Resources.Load<GameObject>("Prefabs/Start"), GameObject.Find("StartPoints").transform);
        startPoint.transform.position = transform.position;
        startPoint.name = $"Start {LevelEditorController.CarCount}";

        var targetPoint = Instantiate(Resources.Load<GameObject>("Prefabs/Target"), GameObject.Find("TargetPoints").transform);
        targetPoint.transform.SetLocalPosZ(6);
        targetPoint.name = $"Target {LevelEditorController.CarCount}";
        targetPoint.GetComponent<LevelEditorObject>().SetPositionAfterFrame();
        LevelEditorController.Status = LevelEditorController.EditorStatus.PlacingEndPoint;

        EditorSceneManager.MarkAllScenesDirty();
        EditorSceneManager.SaveOpenScenes();
    }
}

#endif