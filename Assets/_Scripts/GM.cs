using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    private bool win;
    private List<Car> cars = new List<Car>();
    private List<Toggle> flags = new List<Toggle>();

    private IEnumerator coroutine = null;

    [Header("Prefabs")]

    [SerializeField] private GameObject flagPrefab = null;

    [Header("References")]

    [SerializeField] private GameObject layoutObject = null;
    [SerializeField] private GameObject winText = null;

    private void Awake()
    {
        foreach (Car car in FindObjectsOfType<Car>())
        {
            cars.Add(car);
            flags.Add(car.Flag = Instantiate(flagPrefab, layoutObject.transform).GetComponent<Toggle>());
        }
    }

    private void Update()
    {
        bool allTrue = true;

        foreach (Toggle flag in flags)
        {
            if (!flag.isOn)
            {
                allTrue = false;
            }
        }

        if (allTrue && !win)
        {
            win = true;

            coroutine = WinCoroutine();
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator WinCoroutine()
    {
        string text = "JOB WELL DONE!";

        winText.SetActive(true);
        winText.GetComponent<Text>().text = text;

        yield return new WaitForSeconds(5f);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}