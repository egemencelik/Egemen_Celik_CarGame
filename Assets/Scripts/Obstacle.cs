using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Car>())
        {
            EventManager.TriggerEvent(Events.ObstacleCollision, new EventParam());
        }
    }
}