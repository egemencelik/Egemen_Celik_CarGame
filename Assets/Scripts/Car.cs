using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public CarData CarData;
    private bool IsControlledByPlayer { get; set; }

    private Rigidbody2D rb;
    private GameObject start, target;
    private string number;
    private List<CarMovementHistory> movementHistory;
    private int currentMoveIndex;
    private Vector3 firstPos;
    private Quaternion firstRot;

    private void Awake()
    {
        number = name.Split(' ')[1];
        start = GameObject.Find($"Start {number}");
        target = GameObject.Find($"Target {number}");
        transform.position = start.transform.position;
    }

    private void Start()
    {
        movementHistory = new List<CarMovementHistory>();
        rb = GetComponent<Rigidbody2D>();
        IsControlledByPlayer = true;
        firstPos = transform.position;
        firstRot = transform.rotation;
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.ReachTarget, OnReachTarget);
        EventManager.StartListening(Events.ObstacleCollision, OnHitObstacle);
        EventManager.StartListening(Events.LevelWon, OnHitObstacle);
        EventManager.StartListening(Events.StartClick, OnStartClick);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.ReachTarget, OnReachTarget);
        EventManager.StopListening(Events.ObstacleCollision, OnHitObstacle);
        EventManager.StopListening(Events.LevelWon, OnHitObstacle);
        EventManager.StopListening(Events.StartClick, OnStartClick);
    }

    private void Update()
    {
        if (!GameManager.instance.StartClick) return;

        if (IsControlledByPlayer)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(Vector3.forward * CarData.turnPower);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(Vector3.forward * -CarData.turnPower);
            }

            rb.velocity = transform.up * CarData.speed;
            movementHistory.Add(new CarMovementHistory(transform.rotation, transform.position));
        }
        else
        {
            if (movementHistory.Count == 0) return;
            if (currentMoveIndex == movementHistory.Count)
            {
                rb.velocity = Vector2.zero;
                gameObject.layer = LayerMask.NameToLayer("Default");
                return;
            }

            transform.position = movementHistory[currentMoveIndex].position;
            transform.rotation = movementHistory[currentMoveIndex].rotation;
            currentMoveIndex++;
        }
    }

    private void OnStartClick(EventParam param)
    {
        if (IsControlledByPlayer) transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnReachTarget(EventParam param)
    {
        if (param.intParam >= int.Parse(number))
        {
            IsControlledByPlayer = false;
            rb.SetVelocity(0, 0);
            ResetCar();
        }
    }

    private void OnHitObstacle(EventParam param)
    {
        //movementHistory.Clear();
        ResetCar();
        IsControlledByPlayer = int.Parse(number) == GameManager.instance.CurrentCarIndex + 1;
        if (int.Parse(number) == GameManager.instance.CurrentCarIndex + 1) movementHistory.Clear();
    }

    private void ResetCar()
    {
        currentMoveIndex = 0;
        gameObject.layer = LayerMask.NameToLayer("Player");
        transform.position = firstPos;
        transform.rotation = firstRot;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var car = other.transform.GetComponent<Car>();
        if (car)
        {
            EventManager.TriggerEvent(Events.ObstacleCollision, new EventParam());
        }
    }
}