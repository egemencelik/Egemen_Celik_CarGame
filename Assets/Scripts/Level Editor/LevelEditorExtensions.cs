using UnityEngine;

public static class LevelEditorExtensions
{
    public static void SetTransformValuesFromJson(this GameObject go, SingleJsonObject sjo)
    {
        var newPosition = new Vector2(sjo.xPos, sjo.yPos);
        var newRotation = new Quaternion(sjo.xRot, sjo.yRot, sjo.zRot, sjo.wRot);
        var drawLine = go.GetComponent<DrawLines>();

        if (drawLine) drawLine.rot = newRotation;
        
        go.transform.position = newPosition;
        go.transform.rotation = newRotation;
        
    }

    public static void SetPosZ(this Transform tr, float z)
    {
        var newPosition = new Vector3(tr.position.x, tr.position.y, z);
        tr.position = newPosition;
    }
    
    public static void SetLocalPosZ(this Transform tr, float z)
    {
        var newPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, z);
        tr.localPosition = newPosition;
    }
}