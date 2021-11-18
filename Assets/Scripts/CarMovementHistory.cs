using UnityEngine;
public readonly struct CarMovementHistory
{
    public Quaternion rotation { get; }
    public Vector3 position { get; }

    public CarMovementHistory(Quaternion rotation, Vector3 position)
    {
        this.rotation = rotation;
        this.position = position;
    }
}
