using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button ExitButton;
    public Button HelpButton;

    public GameObject SettingsPanel;

    public GameObject PanelWithControls;

    [SerializeField] private GameObject MobileControls;
    [SerializeField] private GameObject PCControls;


    // Start is called before the first frame update
    void Start()
    {


        if (Application.platform == RuntimePlatform.WindowsEditor | Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ExitButton.gameObject.SetActive(true);
            PanelWithControls.SetActive(true);

            MobileControls.SetActive(false);
            PCControls.SetActive(true);
        }
        else
        {
            ExitButton.gameObject.SetActive(false);
            PanelWithControls.SetActive(false);

            MobileControls.SetActive(true);
            PCControls.SetActive(false);

            //move button
            RectTransform NewHelpButtonYPos = HelpButton.GetComponent<RectTransform>();
            NewHelpButtonYPos.anchoredPosition = new Vector3(0, -270, 0);

            //resize back
            RectTransform NewSetPanelHeight = SettingsPanel.GetComponent<RectTransform>();
            NewSetPanelHeight.sizeDelta = new Vector2(600, 380);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
