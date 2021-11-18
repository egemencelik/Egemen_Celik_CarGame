using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    private string number;

    private void Awake()
    {
        number = name.Split(' ')[1];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var car = other.GetComponent<Car>();

        if (!car) return;
        if (!car.name.Split(' ')[1].Equals(number)) return; // Check if correct car
        
        EventManager.TriggerEvent(Events.ReachTarget, new EventParam{intParam = int.Parse(number)});
    }
}
