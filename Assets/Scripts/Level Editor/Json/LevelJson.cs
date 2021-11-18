using System;
using UnityEngine;

[Serializable]
public class LevelJson
{
    public CarBundle[] bundles;
    public SingleJsonObject[] obstacles;

    public LevelJson(CarBundle[] bundles, SingleJsonObject[] obstacles)
    {
        this.bundles = bundles;
        this.obstacles = obstacles;
    }
}

[Serializable]
public class CarBundle
{
    public CarJsonObject car;
    public SingleJsonObject start;
    public SingleJsonObject target;

    public CarBundle(CarJsonObject car, SingleJsonObject start, SingleJsonObject target)
    {
        this.car = car;
        this.start = start;
        this.target = target;
    }
}

[Serializable]
public class SingleJsonObject
{
    public string name;
    public bool isActive;

    public float xPos;
    public float yPos;
    public float xRot;
    public float yRot;
    public float zRot;
    public float wRot;

    private Vector3 position;
    private Quaternion rotation;

    public SingleJsonObject(Transform tr)
    {
        name = tr.name;
        position = tr.position;
        rotation = tr.rotation;
        xPos = position.x;
        yPos = position.y;
        xRot = rotation.x;
        yRot = rotation.y;
        zRot = rotation.z;
        wRot = rotation.w;
        isActive = tr.gameObject.activeInHierarchy;
    }

    public override string ToString()
    {
        return name;
    }
}

[Serializable]
public class CarJsonObject : SingleJsonObject
{
    public string carType;
    public CarJsonObject(Transform tr, string carType) : base(tr)
    {
        this.carType = carType;
    }
}
