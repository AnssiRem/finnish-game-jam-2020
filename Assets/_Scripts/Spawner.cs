using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject car;
    public float timeLeft;
    public bool spawned;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 && spawned == false)
        {
            Instantiate(car, transform.position, transform.rotation);
            spawned = true;
        }
    }

}
