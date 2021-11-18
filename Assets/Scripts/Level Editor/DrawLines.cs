#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DrawLines : MonoBehaviour
{
    [HideInInspector]
    public Quaternion rot = Quaternion.identity;

    public void Update()
    {
        if (EditorApplication.isPlaying) return;
        transform.rotation = rot;
    }
}
#endif