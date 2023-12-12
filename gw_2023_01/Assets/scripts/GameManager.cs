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

    public HeroController HC;

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

        Screen.SetResolution((int)Screen.width, (int)Screen.height, true);
        
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

        UIManager.CounterTextValue = "0"; //start count

        UIManager.GameTypeSelector = 0;

        UIManager.ShowMissText = false;


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

        if (!LooseState && !PauseState)
        {

        switch (Object)
        {
            case 1:
                string TempName;

                TempName = "object1_" + InstObjCount.ToString();

                TempGO01 = Instantiate(gameObj01);     
                TempGO01.name = TempName;

                soundQueue.Add("tone01");

                break;
            case 2:
                
                TempName = "object2_" + InstObjCount.ToString();

                TempGO02 = Instantiate(gameObj02);
                TempGO02.name = TempName;

                soundQueue.Add("tone02");
            
                break;
            case 3:
                
                TempName = "object3_" + InstObjCount.ToString();

                TempGO03 = Instantiate(gameObj03);
                TempGO03.name = TempName;

                soundQueue.Add("tone03");

                break;
            case 4:
                
                TempName = "object4_" + InstObjCount.ToString();

                TempGO04 = Instantiate(gameObj04);
                TempGO04.name = TempName;

                soundQueue.Add("tone04");

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

                if (soundQueue.Count > 0 && !LooseState)
                {
                    //Debug.Log(soundQueue.Count +"/"+ TotalGameTime);

                    _clip = (AudioClip)Resources.Load(soundQueue.Last());
                    SoundManager.Instance.PlaySound(_clip);
                    soundQueue.Remove(soundQueue.First());
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

        soundQueue.Clear();
    }

    void IdleLogic()
    {

        UIManager.CounterTextValue = DateTime.Now.ToString("HH:mm");

        GameTime = GameTime + Time.unscaledDeltaTime;

        ScoreCounter();

        //99 for idle cycle
        if (GameScore == 99)
        {
            GameScore = 0;
        }

        if (GameTime > 1f)
        {
            int CurrentHeroDirection = HeroDirection;
            HeroDirection = UnityEngine.Random.Range(0, 4);

            if (CurrentHeroDirection != HeroDirection) { HC.hideObect();}

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
            if (!IdleState) { UIManager.CounterTextValue = GameScore.ToString(); } //for idle
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
            UIManager.CounterTextValue = " " + ScoreFirstPartString + " " + ScoreSecondPartString;
        }
    }

    public void ScoreCounter()
    {
        //TotalGameTime +   + "/" + InstObjCount.ToString()

        ScoreOutput();

        if (LostScore > 0)
        {
            UIManager.ShowMissText = true;
        }

        //max lost value is 6
        if (LostScore >= GameEndValue)
        {
            EndGameState = true;
        }
        
    }


    public void gameA()
    {
        //Debug.Log("Game A");

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

            UIManager.GameTypeSelector = 0;

            GameScore = 0;
            LostScore = 0;

            UIManager.ShowMissText = false; //hide miss text

            GameTime = 0;
            TotalGameTime = 0;
            ShowAddCharEndTime = 0;

            EndGameState = false;
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
                UIManager.GameTypeSelector = 1;
            }
            else
            {
                UIManager.GameTypeSelector = 2;
            }

            UIManager.ShowMissText = false; //hide miss text

            HeroDirection = 2; // bottom right

            Destroy(TempGO01);
            Destroy(TempGO02);
            Destroy(TempGO03);
            Destroy(TempGO04);
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
