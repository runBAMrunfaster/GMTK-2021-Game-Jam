using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject aboutScreen;
    public void PlayGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void AboutScreen()
    {
        mainMenu.SetActive(false);
        aboutScreen.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
        aboutScreen.SetActive(false);
    }
}
