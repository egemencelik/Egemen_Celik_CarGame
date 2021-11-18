using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "Car/Create new car data")]
public class CarData : ScriptableObject
{
    public float speed = 2;
    public float turnPower = .5f;
}
