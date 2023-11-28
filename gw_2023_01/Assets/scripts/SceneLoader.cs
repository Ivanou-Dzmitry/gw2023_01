using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{

    public GameObject MenuPanel;

    public static bool OpenAtPause;

    void Start()
    {
        MenuPanel.SetActive(false);
        OpenAtPause = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenMenu()
    {
        if (MenuPanel != null)
        {
            bool isActive = MenuPanel.activeSelf;
            MenuPanel.SetActive(!isActive);
        }
        //Debug.Log("HERE");
    }

    public void OpenDiallog()
    {

        if (GameManager.PauseState)
        {
            OpenAtPause = true;
        }

        if (!GameManager.PauseState && !OpenAtPause)
        {
            GameManager.PauseState = true;
            GameManager.GameState = false;
        }

        Debug.Log(OpenAtPause);
    }

    public void CloseDiallog()
    {

        if (GameManager.PauseState && !OpenAtPause)
        {
            GameManager.PauseState = false;
            GameManager.GameState = true;            
        }
        
        OpenAtPause = false;
    }
}
