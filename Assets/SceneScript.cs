using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneScript : MonoBehaviour
{
    public static bool isLegacy;
    public void StartGame()
    {
        string button_name = EventSystem.current.currentSelectedGameObject.name;
        if (button_name.Equals("PlayButton"))
            isLegacy = true;
        else
            isLegacy = false;
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
