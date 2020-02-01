using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene("Jannescene");
        }

            public void ExitGame()
            {
                Application.Quit();
            }

    public void Credits()
    {
        credits.SetActive(true);
    }

    public void BackToMainMenu()
    {
        credits.SetActive(false);
    }
        }
    

