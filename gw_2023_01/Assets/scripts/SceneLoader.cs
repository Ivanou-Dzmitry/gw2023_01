using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{

    public GameObject MenuPanel;

    void Start()
    {
        MenuPanel.SetActive(false);
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
}
