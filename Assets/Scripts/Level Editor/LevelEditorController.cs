#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LevelEditorController : MonoBehaviour
{
    public enum EditorStatus
    {
        Empty,
        PlacingCar,
        PlacingObstacle,
        PlacingEndPoint,
        ConnectingStartAndTarget
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            OnEnterPlayMode();
        }
    }

    private static void OnEnterPlayMode()
    {
        var cars = GameObject.Find("Cars").transform;
        var starts = GameObject.Find("StartPoints").transform;
        var targets = GameObject.Find("TargetPoints").transform;
        CheckCounts(cars, starts, targets);
        CheckNames(cars, starts, targets);
    }


    public static EditorStatus Status;

    public static int CarCount
    {
        get
        {
            if (!init)
            {
                init = true;
            }

            GetCurrentCarCount();

            return carCount;
        }

        set => carCount = value;
    }

    private static int carCount;
    private static bool init;

    private static void GetCurrentCarCount() => carCount = GameObject.Find("Cars").transform.childCount;

    private static void CheckCounts(Transform cars, Transform starts, Transform targets)
    {
        var carC = cars.childCount;
        var startC = starts.childCount;
        var targetC = targets.childCount;

        if (carC == 0 || targetC == 0 || startC == 0)
        {
            DisplayError($"Cars: {carC}, Start Points: {startC}, Target Points: {targetC}\nThese objects must have at least 1 child!",
                "Objects must have at least 1 child!");
        }

        if (carC != startC || carC != targetC || targetC != startC)
        {
            DisplayError($"Cars: {carC}, Start Points: {startC}, Target Points: {targetC}\nMake sure these are equal!",
                "Car counts and target counts must be equal!");
        }
    }

    private static void CheckNames(Transform cars, Transform starts, Transform targets)
    {
        var carName = string.Empty;
        for (var i = 0; i < cars.childCount; i++)
        {
            try
            {
                carName = cars.GetChild(i).name;
                var carNumber = carName.Split(' ')[1];
                if (!int.TryParse(carNumber, out _)) DisplayError($"{carName} doesn't have number!", $"{carName} doesn't have number!");
                if (!starts.Find($"Start {carNumber}")) DisplayError($"Can't find Start {carNumber}!", $"Can't find Start {carNumber}!");
                if (!targets.Find($"Target {carNumber}")) DisplayError($"Can't find Target {carNumber}!", $"Can't find Target {carNumber}!");
            }
            catch (IndexOutOfRangeException e)
            {
                DisplayError($"{carName} doesn't have number!",
                    $"{carName} doesn't have number!");
            }
        }
    }

    private static void DisplayError(string msg, string log)
    {
        EditorUtility.DisplayDialog("Level Error",
            msg, "OK");
        EditorApplication.isPlaying = false;
        throw new Exception(log);
    }
}

#endif