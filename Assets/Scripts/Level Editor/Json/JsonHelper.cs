using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class NameComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return string.CompareOrdinal(x?.ToString(), y?.ToString());
    }
}

public static class JsonHelper
{
    public static string CreateLevelJSON()
    {
        var starts = CreateJsonObjects(GameObject.Find("StartPoints"));
        var targets = CreateJsonObjects(GameObject.Find("TargetPoints"));
        var cars = CreateCarJsonObjects(GameObject.Find("Cars"));
        var obstacles = CreateJsonObjects(GameObject.Find("Obstacles"));

        return JsonUtility.ToJson(new LevelJson(CreateBundles(cars, targets, starts), obstacles));
    }

    public static void LoadLevelFromJSON(string json)
    {
        var levelJson = JsonUtility.FromJson<LevelJson>(json);
        GenerateLevel(levelJson);
    }

    private static SingleJsonObject[] CreateJsonObjects(GameObject go)
    {
        return (from Transform child in go.transform select new SingleJsonObject(child)).ToArray();
    }

    private static CarJsonObject[] CreateCarJsonObjects(GameObject go)
    {
        return (from Transform transform in go.transform
            let temp = transform.GetComponent<Car>() 
            let type = temp.CarData.name.Split(' ')[0] 
            select new CarJsonObject(transform, type))
            .ToArray();
    }

    private static CarBundle[] CreateBundles(CarJsonObject[] cars, SingleJsonObject[] targets, SingleJsonObject[] starts)
    {
        var comparer = new NameComparer();
        Array.Sort(cars, comparer);
        Array.Sort(targets, comparer);
        Array.Sort(starts, comparer);

        return cars.Select((t, i) => new CarBundle(t, starts[i], targets[i])).ToArray();
    }

    private static void GenerateLevel(LevelJson json)
    {
        LevelEditorController.CarCount = json.bundles.Length;

        foreach (var bundle in json.bundles)
        {
            var carGO = Object.Instantiate(Resources.Load<GameObject>($"Prefabs/{bundle.car.carType} Car"), GameObject.Find("Cars").transform);
            carGO.name = bundle.car.name;
            carGO.SetActive(bundle.car.isActive);
            carGO.GetComponent<LevelEditorObject>().positionSet = true;
            carGO.SetTransformValuesFromJson(bundle.car);

            var targetGO = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Target"), GameObject.Find("TargetPoints").transform);
            targetGO.name = bundle.target.name;
            targetGO.SetActive(bundle.target.isActive);
            targetGO.GetComponent<LevelEditorObject>().positionSet = true;
            targetGO.SetTransformValuesFromJson(bundle.target);

            var startGO = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Start"), GameObject.Find("StartPoints").transform);
            startGO.name = bundle.start.name;
            startGO.SetActive(bundle.start.isActive);
            startGO.SetTransformValuesFromJson(bundle.start);
        }

        foreach (var obstacle in json.obstacles)
        {
            var obstacleGO = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Obstacle"), GameObject.Find("Obstacles").transform);
            obstacleGO.SetActive(obstacle.isActive);
            obstacleGO.GetComponent<LevelEditorObject>().positionSet = true;
            obstacleGO.SetTransformValuesFromJson(obstacle);
        }
    }
}