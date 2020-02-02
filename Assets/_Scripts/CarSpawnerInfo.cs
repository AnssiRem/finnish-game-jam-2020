using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarSpawnerInfo
{
    [SerializeField] private float spawnTime = 0f;
    [SerializeField] private GameObject car = null;

    public float SpawnTime { get => spawnTime; set => spawnTime = value; }
    public GameObject Car { get => car; }
}
