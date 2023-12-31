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
    public Button MenuButton;

    [SerializeField] private GameObject PlayBanner;
    [SerializeField] private GameObject EndGameBanner;

    public GameObject HelpPanel;

    public GameObject SettingsPanel;
    public GameObject SettingsScreen;

    public GameObject PanelWithControls;

    public bool Win;

    [SerializeField] private GameObject CharacterControls;

    [SerializeField] private GameObject MobileControls;
    [SerializeField] private GameObject PCControls;

    //GAME UI
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TMP_Text MissText;
    [SerializeField] private TMP_Text GameATxt;
    [SerializeField] private TMP_Text GameBTxt;

    public static string CounterTextValue;
    public static bool ShowMissText;

    public static int GameTypeSelector; //0-OFF, 1-A, 2-B

    // Start is called before the first frame update
    void Start()
    {
        PlayBanner.SetActive(true);
        EndGameBanner.SetActive(false);
        SettingsScreen.SetActive(false);

        if (Application.platform == RuntimePlatform.WindowsEditor | Application.platform == RuntimePlatform.WindowsPlayer)
        {
            ExitButton.gameObject.SetActive(true);
            PanelWithControls.SetActive(true);

            MobileControls.SetActive(false);
            PCControls.SetActive(true);

            Win = true;
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

            Win = false;
        }
    }


    void Orientation()
    {
        //get safe area values
        float SaveX = Screen.safeArea.x;
        float SaveY = Screen.safeArea.y;

        //set menu button to safe area
        RectTransform NewMenuButtonXPos = MenuButton.GetComponent<RectTransform>();

        //set controls
        RectTransform NewPlayerControls = CharacterControls.GetComponent<RectTransform>();

        //get screen aspect
        float ScreenAspect = (float)Screen.width / (float)Screen.height;

        if (Screen.orientation == ScreenOrientation.Portrait | Screen.orientation == ScreenOrientation.PortraitUpsideDown && !Win)
        {
            if (ScreenAspect < 1.5f)
                Camera.main.orthographicSize = 19.0f; //square
            else
                Camera.main.orthographicSize = 16.0f; //16:9

            //ipad
            if (ScreenAspect >= 0.69 && ScreenAspect <= 0.75f)
                Camera.main.orthographicSize = 13.0f; //16:9

            NewMenuButtonXPos.anchoredPosition = new Vector3(0, -SaveY, 0);

            NewPlayerControls.offsetMin = new Vector2(0, SaveY);
            NewPlayerControls.offsetMax = new Vector2(0, 0);
        }
        else
        {
            if (ScreenAspect < 1.5f)
                Camera.main.orthographicSize = 8.0f; //square
            else
                Camera.main.orthographicSize = 6.0f; //16:9

            //ipad
            if (ScreenAspect == 0.75f)
                Camera.main.orthographicSize = 12.0f; //16:9

            NewMenuButtonXPos.anchoredPosition = new Vector3(SaveX, 0, 0);

            NewPlayerControls.offsetMin = new Vector2(SaveX, 0);
            NewPlayerControls.offsetMax = new Vector2(-SaveY, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        Orientation();

        // clock, counter value
        counterText.text = CounterTextValue;

        //MISS text
        MissText.enabled = ShowMissText;

        if (GameManager.GameState)
        {
            PlayBanner.SetActive(false);
        }

        if (GameManager.EndGameState)
        {
            EndGameBanner.SetActive(true);
        }
        else
        {
            EndGameBanner.SetActive(false);
        }

        //Show GAME AB text
        switch (GameTypeSelector)
        {
            case 0:
                GameATxt.enabled = false; GameBTxt.enabled = false;
                break;
            case 1:
                GameATxt.enabled = true; GameBTxt.enabled = false;
                break;
            case 2:
                GameATxt.enabled = false; GameBTxt.enabled = true;
                break;
            default:
                break;
        }


        if (UserInput.instance.CancelMenu)
        {
            //hide play banner. its showing only on 1st start
            PlayBanner.SetActive(false);

            if (SettingsScreen.activeInHierarchy)
            {
                SettingsScreen.SetActive(false);
                MenuButton.interactable = true;
                UnPauseGame();
                return;
            }

            //run pause
            if (!SettingsScreen.activeInHierarchy && !HelpPanel.activeInHierarchy)
            {
                SettingsScreen.SetActive(true);
                MenuButton.interactable = false;
                PauseGame();
            }

            //close help panel
            if (HelpPanel.activeInHierarchy)
            {
                HelpPanel.SetActive(false);
                SettingsScreen.SetActive(true);
            }
        }
    }

    public void UnPauseGame()
    {
        GameManager.PauseState = false;

        if (!GameManager.IdleState)
        {
            GameManager.GameState = true;
        }
    }

    public void PauseGame()
    {
        GameManager.PauseState = true;

        if (GameManager.GameState)
        {
            GameManager.GameState = false;
        }
    }
}
