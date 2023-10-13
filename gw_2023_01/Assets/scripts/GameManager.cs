using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GameManager : MonoBehaviour
{
    public GameObject gameObj01;
    public GameObject gameObj02;
    public GameObject gameObj03;
    public GameObject gameObj04;

    public TMP_Text counterText;
    public TMP_Text InfoText;

    [SerializeField] private Button rtButton;
    [SerializeField] private Button rbButton;
    [SerializeField] private Button ltButton;
    [SerializeField] private Button lbButton;

    [SerializeField] private Sprite LargeButtonPressed;
    [SerializeField] private Sprite LargeButtonIdle;

    GameObject TempGO01, TempGO02, TempGO03, TempGO04;
    GameObject MissIcon01, MissIcon02, MissIcon03;

    public static bool IdleState, GameState, LooseState, PauseState, EndGame;

    private int TotalGameTime, RandomObject, InstObjCount;

    private float GameTime;

    List<int> GameplayRanges = new List<int>();

    //player direction
    public static int HeroDirection;
    
    //Score
    private int GameScore;
    private int LostScore;
    public static bool ObjectCollected;

    private readonly int GameObjectsCount = 4; //game objects in current game

    private readonly List<int> objectsOrder = new List<int>(); //for random order

    public static string ObjNameToDelete;

    void Start()
    {
        //for random order
        randomGameObjOrder();

        //idle sate of game
        IdleState = true;
        EndGame = false;

        counterText.text = "0"; //start count

        HeroDirection = 2; // bottom right

        Application.targetFrameRate = 15;

        TotalGameTime = 0;
        GameScore = 0;
        LostScore = 0;

        MissIcon01 = GameObject.Find("lost_fish_1");
        MissIcon01.SetActive(false);

        MissIcon02 = GameObject.Find("lost_fish_2");
        MissIcon02.SetActive(false);

        MissIcon03 = GameObject.Find("lost_fish_3");
        MissIcon03.SetActive(false);

        //set resoluton
        Screen.SetResolution(1920, 1080, true);

        //Gameplay Ranges
        GameplayRanges.Add(5);
        GameplayRanges.Add(10);
        GameplayRanges.Add(15);
    }




    //choose random order of game objects
    void randomGameObjOrder()
    {
        int number;
        for (int i = 0; i < GameObjectsCount; i++)
        {
            do
            {
                number = UnityEngine.Random.Range(1, 5);
            } while (objectsOrder.Contains(number));
            objectsOrder.Add(number);
        }
    }


    void RandomObjectRender()
    {
        int RandomPair;

        RandomPair = UnityEngine.Random.Range(0, 2);

        if (RandomPair == 0)
        {
            RandomObject = UnityEngine.Random.Range(1, 3);
        }
        else
        {
            RandomObject = UnityEngine.Random.Range(3, 5);
        }

        
        objectForRender(RandomObject);
        //Debug.Log(RandomPair + "/" + RandomObject);
    }


    void GameLogic()
    {
        //Debug.Log(TotalGameTime);

        //IntroGameplay();

        //random every odd sec
        if (GameScore >= 0 && GameScore < GameplayRanges[0] && TotalGameTime % 4 == 0)
        {
            RandomObjectRender();
        }

        if (GameScore > GameplayRanges[0] && GameScore < GameplayRanges[1] && TotalGameTime % 3 == 0)
        {
            RandomObjectRender();
        }

        if (GameScore > GameplayRanges[1] && GameScore < GameplayRanges[2] && TotalGameTime % 2 == 0)
        {
            RandomObjectRender();
        }

    }

    void IntroGameplay()
    {

        if (TotalGameTime == 1)
        {
            objectForRender(objectsOrder[0]);
        }

        if (TotalGameTime == 7)
        {
            objectForRender(objectsOrder[1]);
        }

        if (TotalGameTime == 12)
        {
            objectForRender(objectsOrder[2]);
        }

        if (TotalGameTime == 17)
        {
            objectForRender(objectsOrder[3]);
        }

    }


    void objectForRender(int Object)
    {
        InstObjCount++;

        switch (Object)
        {
            case 1:
                string TempName;

                TempName = "object1_" + InstObjCount.ToString();

                TempGO01 = Instantiate(gameObj01);     
                TempGO01.name = TempName;

                //Debug.Log(TempName + " was created!");

                break;
            case 2:
                
                TempName = "object2_" + InstObjCount.ToString();

                TempGO02 = Instantiate(gameObj02);
                TempGO02.name = TempName;

                break;
            case 3:
                
                TempName = "object3_" + InstObjCount.ToString();

                TempGO03 = Instantiate(gameObj03);
                TempGO03.name = TempName;

                break;
            case 4:
                
                TempName = "object4_" + InstObjCount.ToString();

                TempGO04 = Instantiate(gameObj04);
                TempGO04.name = TempName;

                break;
            default:
                break;
        }
    }

    
    void Update()
    {
        
        //listen kbd
        ButtonInput();

        //check speed
        SpeedController();

        if (GameState)
        {
            GameTime = GameTime + Time.unscaledDeltaTime;

            InfoText.gameObject.SetActive(false);

            ScoreCounter();

            if (GameTime > 1f)
            {
                //Debug.Log("TotalGameTime: " + TotalGameTime);
                //Debug.Log("GameTime: " + GameTime);

                GameLogic();

                TotalGameTime++;
                GameTime = 0;
            }

        }

        if (EndGame)
        {
            GameState = false;
            IdleState = true;
            InfoText.text = "Game Over!";
            InfoText.gameObject.SetActive(true);
        }
        

        //Debug.Log(HeroDirection +"/"+ FinalObjectFrame[0]);

    }

    public void ScoreCounter()
    {
        //TotalGameTime +   + "/" + InstObjCount.ToString()
        counterText.text = GameScore.ToString();

        if (ObjNameToDelete != null)
        {
            if (ObjectCollected)
            {
                GameScore++;
                ObjectCollected = false;
            }
            else
            {
                LostScore++;
            }

            switch (LostScore)
            {
                case 1:
                    MissIcon01.SetActive(true);
                    break;
                case 2:
                    MissIcon02.SetActive(true);
                    break;
                case 3:
                    MissIcon03.SetActive(true);
                    break;
                default:
                    break;
            }


            if (LostScore == 3)
            {
                EndGame = true;
            }

            //Debug.Log("ObjNameToDelete is " + ObjNameToDelete);
            GameObject toDestroy = GameObject.Find(ObjNameToDelete);
            //toDestroy.gameObject.SetActive(false);
            Destroy(toDestroy);
            ObjNameToDelete =null;
        }
        

    }

    public void StartGame()
    {
        GameState = true;

        IdleState = false;

        PauseState = false;
    }

    public void PauseGame()
    {
        Debug.Log("Before / " + GameState + "/" + IdleState + "/" + PauseState);

        if (GameState == false && IdleState == false && PauseState == true)
        {
            PauseState = false;
            GameState = true;
        }

        if (GameState == true && IdleState == false && PauseState == false)
        {
            PauseState = true;
            GameState = false;

            InfoText.gameObject.SetActive(true);
            InfoText.text = "Game paused. Press Enter for start.";
        }


        //Debug.Log("After / " + GameState +"/"+ IdleState +"/"+ PauseState);
    }

    public void HeroLeftTop()
    {
        HeroDirection = 0; //left top
        ltButton.image.sprite = LargeButtonPressed;

    }

    public void HeroLeftBottom()
    {
        HeroDirection = 1; //left bottom
        lbButton.image.sprite = LargeButtonPressed;
    }

    public void HeroRightTop()
    {
        HeroDirection = 3; //right top    
        rtButton.image.sprite = LargeButtonPressed;
    }

    public void HeroRightBottom()
    {
        HeroDirection = 2; //right bottom
        rbButton.image.sprite = LargeButtonPressed;
    }


    void ButtonInput()
    {
        if (GameState)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HeroLeftTop();
            }
            else
            {
                ltButton.image.sprite = LargeButtonIdle;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                HeroLeftBottom();
            }
            else
            {
                lbButton.image.sprite = LargeButtonIdle;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                HeroRightBottom();
            }
            else
            {
                rbButton.image.sprite = LargeButtonIdle;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                HeroRightTop();
            }
            else
            {
                rtButton.image.sprite = LargeButtonIdle;
            }

        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
            //Debug.Log("Game Start");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void SpeedController()
    {

        if (InstObjCount < 25)
        {
            Time.timeScale = 1.0f;
            //Debug.Log("Speed 1");
        }
        /*
        if (InstObjCount > 25 && InstObjCount < 50)
        {
            Time.timeScale = 1.25f;
            Debug.Log("Speed 2");
        }

        if (InstObjCount > 50 && InstObjCount < 75)
        {
            Time.timeScale = 1.5f;
            Debug.Log("Speed 3");
        }

        if (InstObjCount > 75 && InstObjCount < 100)
        {
            Time.timeScale = 1.75f;
            Debug.Log("Speed 4");
        }
        */
    }
}
