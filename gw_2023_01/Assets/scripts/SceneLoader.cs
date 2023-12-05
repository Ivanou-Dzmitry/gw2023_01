using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    public GameObject MenuPanel;

    public Button ExitButton;

    public static bool OpenAtPause, DiallogIsOpen;

    void Start()
    {
        MenuPanel.SetActive(false);

        OpenAtPause = false;
        DiallogIsOpen = false;

        //Debug.Log(Application.platform);

        //turn of xit button for android
        if (Application.platform == RuntimePlatform.WindowsEditor | Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ExitButton.gameObject.SetActive(true);
        } else
        {
            ExitButton.gameObject.SetActive(false);
        }

        
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
        DiallogIsOpen = true;

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

        DiallogIsOpen = false;
    }
}
