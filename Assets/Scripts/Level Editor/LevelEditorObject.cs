#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
public abstract class LevelEditorObject : MonoBehaviour
{
    public bool positionSet = true;
    private static readonly WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();

    protected virtual void OnMouseClick()
    {
        positionSet = true;
        LevelEditorController.Status = LevelEditorController.EditorStatus.Empty;
        transform.SetLocalPosZ(6);
        EditorSceneManager.MarkAllScenesDirty();
        EditorSceneManager.SaveOpenScenes();
    }

    protected virtual void OnEnable()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }

        SceneView.duringSceneGui += OnScene;
    }

    private IEnumerator SetPositionAfterFrameCoroutine()
    {
        yield return waitFrame;
        positionSet = true;
    }

    public void SetPositionAfterFrame()
    {
        StartCoroutine(SetPositionAfterFrameCoroutine());
    }

    void OnScene(SceneView scene)
    {
        if (PrefabStageUtility.GetCurrentPrefabStage()) return; // Don't trace events if in prefab mode
        if (positionSet) return;

        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0) // Save position when user clicks
        {
            OnMouseClick();
            e.Use();
        }
        else if (e.type == EventType.MouseMove) // Move object to mouse position
        {
            Vector3 mousePosition = e.mousePosition;
            mousePosition.y = scene.camera.pixelHeight - mousePosition.y;
            mousePosition = scene.camera.ScreenToWorldPoint(mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
            transform.SetLocalPosZ(6);
        }
    }
}

#endif