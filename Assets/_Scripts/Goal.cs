using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private List<Car> cars = new List<Car>();

    private void OnTriggerEnter(Collider other)
    {
        Car collidingCar = null;
        if (other.TryGetComponent<Car>(out collidingCar))
        {
            if (cars.Contains(collidingCar))
            {
                collidingCar.Flag.isOn = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (Car car in cars)
        {
            Gizmos.DrawLine(transform.position, car.transform.position);
        }
    }
}
