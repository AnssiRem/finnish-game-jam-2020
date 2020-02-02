using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private List<CarSpawnerInfo> carInfos = new List<CarSpawnerInfo>();

    void Start()
    {
        foreach (CarSpawnerInfo carInfo in carInfos)
        {
            carInfo.Car.SetActive(false);
        }
    }

    void Update()
    {
        if (carInfos.Count > 0)
        {
            foreach (CarSpawnerInfo spawnerInfo in carInfos)
            {
                spawnerInfo.SpawnTime -= Time.deltaTime;

                if (spawnerInfo.SpawnTime <= 0)
                {
                    spawnerInfo.Car.SetActive(true);
                }
            }
        }
    }
}
