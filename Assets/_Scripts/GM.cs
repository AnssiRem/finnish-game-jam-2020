using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    private List<Car> cars = new List<Car>();
    private List<Toggle> flags = new List<Toggle>();

    [Header("Prefabs")]

    [SerializeField] private GameObject flagPrefab = null;

    [Header("References")]

    [SerializeField] private GameObject layoutObject = null;

    void Awake()
    {
        foreach (Car car in FindObjectsOfType<Car>())
        {
            cars.Add(car);
            flags.Add(car.Flag = Instantiate(flagPrefab, layoutObject.transform).GetComponent<Toggle>());
        }
    }

    void Update()
    {
        bool allTrue = true;

        foreach (Toggle flag in flags)
        {
            if (!flag.isOn)
            {
                allTrue = false;
            }
        }

        if (allTrue)
        {
            print("YOU DA WINNER!");
        }
    }
}
