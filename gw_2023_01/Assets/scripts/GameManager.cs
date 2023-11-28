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

    //txtblock
    public TMP_Text counterText;
    public TMP_Text MissText;
    public TMP_Text InfoText;

    public TMP_Text GameATxt;
    public TMP_Text GameBTxt;

    GameObject TempGO01, TempGO02, TempGO03, TempGO04;

    public static bool IdleState, GameState, LooseState, PauseState, EndGameState, ShowAddChar, CatchState;
    public static bool ObjectCollected;

    private int RandomObject, InstObjCount, ShowAddCharEndTime, GameEndValue;
    public static int GameScore;    //Score
    public static int LostScore;

    public static int TotalGameTime;
    
    public static int HeroDirection; //player direction

    private float GameTime;

    List<int> GameplayRanges = new List<int>();

    private readonly int GameObjectsCount = 4; //game objects in current game

    private readonly List<int> objectsOrder = new List<int>(); //for random order

    public static string ObjNameToDelete;
    public static string LooseObjectName;

    public static string ObjectSound;

    public static List<string> soundQueue = new List<string>(); //for random order

    private AudioClip _clip;

    void Start()
    {   
        //Set Framerate
        Application.targetFrameRate = 15;
        
        //set resoluton
        Screen.SetResolution(1920, 1080, true);

        GameInit();
    }

    void GameInit()
    {
        //for random order
        randomGameObjOrder();

        //idle sate of game
        IdleState = true;

        EndGameState = false;
        ShowAddChar = false;
        LooseState = false;
        CatchState = false;

        counterText.text = "0"; //start count

        GameATxt.enabled = false;
        GameBTxt.enabled = false;
        MissText.enabled = false;

        HeroDirection = 2; // bottom right

        TotalGameTime = 0;
        GameScore = 0;
        LostScore = 0;

        //initial Gameplay Ranges
        GameplayRanges.Add(5);
        GameplayRanges.Add(10);
        GameplayRanges.Add(15);

        //when game is end
        GameEndValue = 6;

        InfoText.text = "Time mode. Press GAME A or B button to start game";
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

        //pair ofobjects
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

        //IntroGameplay();
       //RandomObjectRender();
        
        if (GameScore >= 0 && GameScore <= GameplayRanges[0] && TotalGameTime % 4 == 0)
        {
            RandomObjectRender();
            //Debug.Log("Range 1");
        }

        if (GameScore > GameplayRanges[0] && GameScore <= GameplayRanges[1] && TotalGameTime % 3 == 0)
        {
            RandomObjectRender();
           // Debug.Log("Range 2");
        }

        if (GameScore > GameplayRanges[1] && GameScore <= GameplayRanges[2] && TotalGameTime % 2 == 0)
        {
            RandomObjectRender();
            //Debug.Log("Range 3");
        }
       
        //Debug.Log(GameScore +" / " + "GR1/" + GameplayRanges[1] + " GR2/" + GameplayRanges[2]);

    }

    void IntroGameplay()
    {

        if (TotalGameTime == 0)
        {
            objectForRender(objectsOrder[0]);
            //Debug.Log("Render 1");
        }

        if (TotalGameTime == 6)
        {
            objectForRender(objectsOrder[1]);
        }

        if (TotalGameTime == 11)
        {
            objectForRender(objectsOrder[2]);
        }

        if (TotalGameTime == 16)
        {
            objectForRender(objectsOrder[3]);
        }

    }


    void objectForRender(int Object)
    {
        InstObjCount++;

        if (!LooseState)
        {

        switch (Object)
        {
            case 1:
                string TempName;

                TempName = "object1_" + InstObjCount.ToString();

                TempGO01 = Instantiate(gameObj01);     
                TempGO01.name = TempName;

                if (!soundQueue.Contains("tone01"))
                {
                    soundQueue.Add("tone01");
                }
                        
                //Debug.Log(TempName + " was created!");

                break;
            case 2:
                
                TempName = "object2_" + InstObjCount.ToString();

                TempGO02 = Instantiate(gameObj02);
                TempGO02.name = TempName;

                if (!soundQueue.Contains("tone02"))
                {
                    soundQueue.Add("tone02");
                }

                    //Debug.Log(TempName + " was created!");

                    break;
            case 3:
                
                TempName = "object3_" + InstObjCount.ToString();

                TempGO03 = Instantiate(gameObj03);
                TempGO03.name = TempName;

                    if (!soundQueue.Contains("tone03"))
                    {
                        soundQueue.Add("tone03");
                    }

                    //Debug.Log(TempName + " was created!");

                    break;
            case 4:
                
                TempName = "object4_" + InstObjCount.ToString();

                TempGO04 = Instantiate(gameObj04);
                TempGO04.name = TempName;

                    if (!soundQueue.Contains("tone04"))
                    {
                        soundQueue.Add("tone04");
                    }

                    //Debug.Log(TempName + " was created!");

                    break;
            default:
                break;

        }

        //show additional sharacter
        if (ShowAddCharEndTime > TotalGameTime)
        {
            ShowAddChar = true;
            //Debug.Log("Render Add");
        } else
        {
            ShowAddChar = false;
        }

        }
    }

    
    void Update()
    {
        //listen kbd
        ButtonInput();

        //check speed
        SpeedController();

        //show clock on IDLE state
        if (IdleState)
        {
            IdleLogic();
        }

        //END GAME
        if (EndGameState)
        {
            EndGameLogic();
        }

        //GAME
        if (GameState)
        {
            GameTime = GameTime + Time.unscaledDeltaTime;

            ScoreCounter();

            //1 sec
            if (GameTime > 1f) 
            {
                GameLogic();

                string q1 = "";
                for (int i = 0; i < soundQueue.Count; i++)
                {
                    q1 = q1 + ", " + soundQueue[i];
                    //Debug.Log("Sound:" + q1 + "/ " + TotalGameTime);
                }

                Debug.Log("Q: " + q1 + "/" + LooseState);

                if (soundQueue.Count > 0 && !LooseState)
                {
                    _clip = (AudioClip)Resources.Load(soundQueue[0]);
                    SoundManager.Instance.PlaySound(_clip);
                    soundQueue.Clear();
                }

                TotalGameTime++;

                AdditionalCharacterCall(TotalGameTime);

                GameTime = 0;
            }
        }
    }

    void EndGameLogic()
    {

        GameState = false;
        PauseState = false;
        LooseState = false;

        InfoText.text = "Game Over! Press GAME A or B button to start new game.";
        
        InfoText.gameObject.SetActive(true);
    }

    void IdleLogic()
    {
        counterText.text = DateTime.Now.ToString("HH:mm");

        GameTime = GameTime + Time.unscaledDeltaTime;

        ScoreCounter();

        //99 for idle cycle
        if (GameScore == 99)
        {
            GameScore = 0;
        }

        if (GameTime > 1f)
        {
            HeroDirection = UnityEngine.Random.Range(0, 4);

            GameLogic();

            TotalGameTime++;

            AdditionalCharacterCall(TotalGameTime);

            GameTime = 0;
        }
    }

    void AdditionalCharacterCall(int Time)
    {
        // each 10 sec show second sharacter


        if (Time % 10 == 0)
        {
            int RandomAddCharTime;
            RandomAddCharTime = UnityEngine.Random.Range(4, 6);

            ShowAddCharEndTime = TotalGameTime + RandomAddCharTime;
        }
    }

    void ScoreOutput()
    {
        if (GameScore < 100)
        {
            if (!IdleState) { counterText.text = GameScore.ToString(); } //for idle
        }
        else
        {
            int hundredDigit = (int)Math.Abs(GameScore / 100 % 10); // get hundreds

            string ScoreFirstPartString = hundredDigit.ToString();

            int SecondPartInt = GameScore - (hundredDigit * 100); //get tens
            string ScoreSecondPartString = SecondPartInt.ToString();

            if (ScoreSecondPartString == "0")
            {
                ScoreSecondPartString = "00";
            }

            if (SecondPartInt <= 9 && GameScore > 100)
            {
                ScoreSecondPartString = "0" + ScoreSecondPartString;
            }
            //need because this used for clock
            counterText.text = " " + ScoreFirstPartString + " " + ScoreSecondPartString;
        }
    }

    public void ScoreCounter()
    {
        //TotalGameTime +   + "/" + InstObjCount.ToString()

        ScoreOutput();

        //max lost value is 6
        if (LostScore >= GameEndValue)
        {
            EndGameState = true;
        }
        
    }


    public void gameA()
    {
        if (!GameState)
        {
            StartGame("A");
        }

    }

    public void gameB()
    {
        if (!GameState)
        {
            StartGame("B");
        }
    }


    public void idleState()
    {
        if (EndGameState)
        {
            IdleState = true;

            GameATxt.enabled = false;
            GameBTxt.enabled = false;

            GameScore = 0;
            LostScore = 0;

            MissText.enabled = false; //hide miss text

            GameTime = 0;
            TotalGameTime = 0;
            ShowAddCharEndTime = 0;

            EndGameState = false;

            InfoText.gameObject.SetActive(true);
            InfoText.text = "Time mode. Press GAME A or B button to start game";
        }
    }

    //start new game routine
    void StartGame(string GameType)
    {
            if (!PauseState)
            {

            soundQueue.Clear();

            GameTime = 0;
            TotalGameTime = 0;
            ShowAddCharEndTime = 0;

            GameState = true;

            IdleState = false;

            LooseState = false;

            EndGameState = false;

            GameScore = 0;
            LostScore = 0;

            ObjectSound = null;

            //game type
            if (GameType == "A")
            {
                GameATxt.enabled = true;
                GameBTxt.enabled = false;
            }
            else
            {
                GameBTxt.enabled = true;
                GameATxt.enabled = false;
            }

            MissText.enabled = false; //hide miss text

            HeroDirection = 2; // bottom right

            Destroy(TempGO01);
            Destroy(TempGO02);
            Destroy(TempGO03);
            Destroy(TempGO04);

            InfoText.gameObject.SetActive(false);

        }

    }

    public void PauseGame()
    {
        if (!GameState && !IdleState && PauseState)
        {
            PauseState = false;
            GameState = true;
            InfoText.gameObject.SetActive(false);
        } else if (GameState && !IdleState && !PauseState)
        {
            PauseState = true;
            GameState = false;

            InfoText.gameObject.SetActive(true);
            InfoText.text = "Game Paused";
        }
    }


    void ButtonInput()
    {

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
