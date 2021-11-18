using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.String;

public class GameManager : MonoBehaviour
{
    public int CarCount { get; private set; }
    public string FirstNumber { get; private set; }
    public bool StartClick { get; private set; }
    private GameObject cars, starts, targets;
    public int CurrentCarIndex { get; private set; }

    private static GameManager gameManager;

    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.LogError("There needs to be one active GameManager script on a GameObject in your scene.");
                }
            }

            return gameManager;
        }

        set => gameManager = value;
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.ReachTarget, OnReachTarget);
        EventManager.StartListening(Events.ObstacleCollision, OnHitObstacle);
        EventManager.StartListening(Events.LevelWon, OnLevelWon);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.ReachTarget, OnReachTarget);
        EventManager.StopListening(Events.ObstacleCollision, OnHitObstacle);
        EventManager.StopListening(Events.LevelWon, OnLevelWon);
    }

    private void OnReachTarget(EventParam param)
    {
        if (CurrentCarIndex == CarCount - 1)
        {
            EventManager.TriggerEvent(Events.LevelWon, new EventParam());
            return;
        }

        DisableCurrentPoints();
        EnableNextObjects();
        StartClick = false;
    }

    private void OnHitObstacle(EventParam param)
    {
        DisableAllExceptIndexBatch(CurrentCarIndex);
        DisableHigherIndexChildren(cars.transform, CurrentCarIndex);
        StartClick = false;
    }

    private static void OnLevelWon(EventParam param)
    {
        var index = SceneManager.GetActiveScene().buildIndex + 1;
        index %= SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(index);
    }

    private void EnableNextObjects()
    {
        CurrentCarIndex++;
        cars.transform.GetChild(CurrentCarIndex).gameObject.SetActive(true);
        starts.transform.GetChild(CurrentCarIndex).gameObject.SetActive(true);
        targets.transform.GetChild(CurrentCarIndex).gameObject.SetActive(true);
    }

    private void DisableCurrentPoints()
    {
        starts.transform.GetChild(CurrentCarIndex).gameObject.SetActive(false);
        targets.transform.GetChild(CurrentCarIndex).gameObject.SetActive(false);
    }

    private void Start()
    {
        ReorderObjects();

        cars = GameObject.Find("Cars");
        starts = GameObject.Find("StartPoints");
        targets = GameObject.Find("TargetPoints");

        CarCount = cars.transform.childCount;
        CurrentCarIndex = 0;
        FirstNumber = null;

        DisableAllExceptIndexBatch(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !StartClick)
        {
            EventManager.TriggerEvent(Events.StartClick, new EventParam());
            StartClick = true;
        }
    }

    private void DisableAllExceptIndexBatch(int index)
    {
        DisableAllExceptIndexChild(cars.transform, index);
        DisableAllExceptIndexChild(starts.transform, index);
        DisableAllExceptIndexChild(targets.transform, index);
    }

    private void DisableAllExceptIndexChild(Transform tra, int index)
    {
        foreach (Transform tr in tra)
        {
            tr.gameObject.SetActive(false);
        }

        var firstGO = tra.GetChild(index).gameObject;
        FirstNumber ??= firstGO.name.Split(' ')[1];
        firstGO.SetActive(true);
    }

    private void DisableHigherIndexChildren(Transform tra, int index)
    {
        foreach (Transform tr in tra)
        {
            tr.gameObject.SetActive(tr.GetSiblingIndex() <= index);
        }
    }

    // These methods exists here too so we can use them in build.
    private static void ReorderObjects()
    {
        Reorder(GameObject.Find("StartPoints").transform);
        Reorder(GameObject.Find("TargetPoints").transform);
        Reorder(GameObject.Find("Cars").transform);
    }

    private static void Reorder(Transform transform)
    {
        var list = (from Transform tr in transform select tr.name).ToList();
        list.Sort((x, y) => Compare(x, y, StringComparison.Ordinal));

        foreach (Transform tr in transform)
        {
            tr.SetSiblingIndex(list.IndexOf(tr.name));
        }
    }
}