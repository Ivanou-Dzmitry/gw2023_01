using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject gameObj01;
    public GameObject gameObj02;
    public GameObject gameObj03;
    public GameObject gameObj04;

    public HeroController HC;

    private GameObject TempGO01, TempGO02, TempGO03, TempGO04;

    public static string LooseObjectName;

    public static bool IdleState, GameState, LooseState, PauseState, EndGameState, ShowAddChar, CatchState;

    private int RandomObject, InstObjCount, ShowAddCharEndTime, GameEndValue;

    //Score
    public static int GameScore;    
    public static int LostScore;

    public static int TotalGameTime;
    public static int HeroDirection; //player direction

    private float GameTime;
    public static float GameSpeed = 1;
    private float LooseSpeed = 1;
    private float CurrentSpeed;

    private int GameCycles = 10; //how many game cycles
    private int CurrentGameCycle; //currentGameCycle

    private int CycleLenght;
    private int CycleQuarta = 25; //1/4 of cycle

    //initial Gameplay Ranges
    List<int> GameplayRanges = new List<int> ();

    List<float> GameASpeed = new List<float>();
    List<float> GameBSpeed = new List<float>();

    //Speed settings
    private float InitialASpeed = 1.0f, InitialBSpeed = 0.6f;
    private float InitialASpeedStep = 0.1f, InitialBSpeedStep = 0.05f;

    private float SpeedStep = 0.025f;

    private bool wasExecuted = false;

    private readonly int GameObjectsCount = 4; //game objects in current game

    private readonly List<int> objectsOrder = new List<int>(); //for random order

    
    //for audio
    private AudioClip _clip;
    public static string ObjectSound1;

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

        //when game is end
        GameEndValue = 6;

        SetInitialRanges();
    }

    private void SetInitialRanges()
    {
        //set Initial gameplay ranges
        GameplayRanges.Insert(0, CycleQuarta);        
        for (int i = 1; i < 4; i++)
        {
            GameplayRanges.Insert(i, GameplayRanges[0] * (i+1));
        }

        CycleLenght = GameplayRanges[3];

        //set speed A initial values
        GameASpeed.Insert(0, InitialASpeed);
        for (int i = 1; i < 5; i++)
        {
            GameASpeed.Insert(i, GameASpeed[i - 1] - InitialASpeedStep);
        }

        //set speed B initial values
        GameBSpeed.Insert(0, InitialBSpeed);
        for (int i = 1; i < 5; i++)
        {
            GameBSpeed.Insert(i, GameBSpeed[i - 1] - InitialBSpeedStep);
        }

        //zero game cycle
        CurrentGameCycle = 0;
        
    }


    //choose random order of game objects
    void randomGameObjOrder()
    {
        int number;
        for (int i = 0; i < GameObjectsCount; i++)
        {
            do
            {
                number = UnityEngine.Random.Range((int)1, (int)5);
            } while (objectsOrder.Contains(number));
            objectsOrder.Add(number);
        }
    }


    void RandomObjectRender()
    {
        RandomObject = UnityEngine.Random.Range((int)1, (int)5);
        objectForRender(RandomObject);
    }


    void GameLogic()
    {

        //intro simple gameplay
        if (TotalGameTime < 23 && CurrentGameCycle == 0)
        {
            IntroGameplay();
        }

        //1 fish 4sec
        if (TotalGameTime > 23 | CurrentGameCycle > 0)
        {
            //stage 1
            if (GameScore >= 1 && GameScore <= GameplayRanges[0] && TotalGameTime % 4 == 0)
            {
                if (UIManager.GameTypeSelector == 1)
                {
                    CurrentSpeed = GameASpeed[1];
                }
                else
                {
                    CurrentSpeed = GameBSpeed[1];
                }

                RandomObjectRender();
            }

            ////stage 2 - 1 fish 3sec
            if (GameScore > GameplayRanges[0] && GameScore <= GameplayRanges[1] && TotalGameTime % 3 == 0)
            {
                if (UIManager.GameTypeSelector == 1)
                {
                    CurrentSpeed = GameASpeed[2];
                }
                else
                {
                    CurrentSpeed = GameBSpeed[2];
                }

                RandomObjectRender();
            }

            ////stage 3 - 1 fish 2sec
            if (GameScore > GameplayRanges[1] && GameScore <= GameplayRanges[2] && TotalGameTime % 2 == 0)
            {
                if (UIManager.GameTypeSelector == 1)
                {
                    CurrentSpeed = GameASpeed[3];
                }
                else
                {
                    CurrentSpeed = GameBSpeed[3];
                }

                RandomObjectRender();
            }

            //stage 4
            if (GameScore > GameplayRanges[2] && GameScore <= GameplayRanges[3] && TotalGameTime % 2 == 0)
            {

                if (UIManager.GameTypeSelector == 1)
                {
                    CurrentSpeed = GameASpeed[4];
                }
                else
                {
                    CurrentSpeed = GameBSpeed[4];
                }

                RandomObjectRender();

                //reset status for loop
                wasExecuted = false;
            }
        }


    }

    void IntroGameplay()
    {

        if (TotalGameTime == 0)
        {
            objectForRender(objectsOrder[0]);
        }

        if (TotalGameTime == 6)
        {
            objectForRender(objectsOrder[1]);
        }

        if (TotalGameTime == 12)
        {
            objectForRender(objectsOrder[2]);
        }

        if (TotalGameTime == 18)
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

                    TempName = "obj_01_" + InstObjCount.ToString();
                    TempGO01 = Instantiate(gameObj01);     
                    TempGO01.name = TempName;

                    break;
                case 2:
                
                    TempName = "obj_02_" + InstObjCount.ToString();
                    TempGO02 = Instantiate(gameObj02);
                    TempGO02.name = TempName;
            
                    break;
                case 3:
                
                    TempName = "obj_03_" + InstObjCount.ToString();
                    TempGO03 = Instantiate(gameObj03);
                    TempGO03.name = TempName;

                    break;
                case 4:
                
                    TempName = "obj_04_" + InstObjCount.ToString();
                    TempGO04 = Instantiate(gameObj04);
                    TempGO04.name = TempName;

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

            if (LooseState)
            {
                GameSpeed = LooseSpeed;
            }
            else
            {
                GameSpeed = CurrentSpeed;
            }            

            //Game Speed
            if (GameTime > GameSpeed) 
            {
                MyDebug("Update");

                GameCyclesLogic();
                
                GameLogic();

                SoundLogic();

                if (!LooseState)
                {
                    TotalGameTime++;                
                }
                    
                AdditionalCharacterCall(TotalGameTime);

                GameTime = 0;
            }
        }
    }

    void SoundLogic()
    {
        int ObjectsCount = 0;
        string forsound = "";

        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("fish");
        foreach (GameObject obj in allObjects)
        {
            ObjectsCount++;
            forsound = obj.name;
        }

        //Debug.Log(TotalGameTime + "/" + LooseState);

        if (ObjectsCount == 1 && !LooseState)
        {
            if (forsound.Contains("obj_01"))
            {
                _clip = (AudioClip)Resources.Load(SoundManager.ObjectSounds[0]);
                SoundManager.Instance.PlaySound(_clip);
            }

            if (forsound.Contains("obj_02"))
            {
                _clip = (AudioClip)Resources.Load(SoundManager.ObjectSounds[1]);
                SoundManager.Instance.PlaySound(_clip);
            }

            if (forsound.Contains("obj_03"))
            {
                _clip = (AudioClip)Resources.Load(SoundManager.ObjectSounds[2]);
                SoundManager.Instance.PlaySound(_clip);
            }

            if (forsound.Contains("obj_04"))
            {
                _clip = (AudioClip)Resources.Load(SoundManager.ObjectSounds[3]);
                SoundManager.Instance.PlaySound(_clip);
            }

        }

        if (ObjectsCount > 1 && !LooseState)
        {
            //play random sound
            int RandomSoundNumber = UnityEngine.Random.Range((int)0, (int)4);
            _clip = (AudioClip)Resources.Load(SoundManager.ObjectSounds[RandomSoundNumber]);
            SoundManager.Instance.PlaySound(_clip);
        }
    }

    void EndGameLogic()
    {
        GameState = false;
        PauseState = false;
        LooseState = false;
    }

    void IdleLogic()
    {
        //clock
        UIManager.CounterTextValue = DateTime.Now.ToString("HH:mm");

        GameTime = GameTime + Time.unscaledDeltaTime;

        ScoreCounter();

        //99 for idle cycle
        if (GameScore == 99)
        {
            GameScore = 0;
        }

        if (GameTime > GameSpeed)
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

    private void GameCyclesLogic()
    {
        if (!wasExecuted)
        {
            //game cycles 100, 200, 300 etc
            if (GameScore > 0 && GameScore % CycleLenght == 0)
            {   
                CurrentGameCycle++;

                //add next range values
                for (int i = 0; i < GameplayRanges.Count; i++)
                {
                    int CurrentVal = GameplayRanges[i];
                    GameplayRanges[i] = CycleLenght + CurrentVal;   
                }
                
                //add speed A
                for (int i = 0; i < GameASpeed.Count; i++)
                {
                    float CurrentVal = GameASpeed[i];
                    GameASpeed[i] = CurrentVal - SpeedStep;
                }

                //add speed B
                for (int i = 0; i < GameBSpeed.Count; i++)
                {
                    float CurrentVal = GameBSpeed[i];
                    GameBSpeed[i] = CurrentVal - SpeedStep;
                }
            }

            wasExecuted = true;
        }

        //game loop
        if (CurrentGameCycle == GameCycles)
        {
            LostScore = 0;
            GameScore = 0;
            TotalGameTime = 0;

            if (UIManager.GameTypeSelector == 1)
            {
                CurrentSpeed = GameASpeed[0];
            }
            else
            {
                CurrentSpeed = GameBSpeed[0];
            }

            UIManager.ShowMissText = false;

            SetInitialRanges();

            DeleteTempGO();
        }
    }

    public void ScoreCounter()
    {
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
         if (!GameState)
        {
            StartGame("A");
            CurrentSpeed = GameASpeed[0]; //normal speed
        }
    }

    public void gameB()
    {
        if (!GameState)
        {
            StartGame("B");
            CurrentSpeed = GameBSpeed[0];//fastest speed
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

    private void DeleteTempGO()
    {
        try
        {
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag("fish");
            foreach (GameObject obj in allObjects)
            {
                Destroy(obj);
            }
        }
        catch (Exception)
        {

            throw;
        }

    }

    //start new game routine
    void StartGame(string GameType)
    {
        if (!PauseState)
        {
            if (LooseState)
            {
                LooseState = false;
            }

            gameObj01.SetActive(false);

            //delete objects
            DeleteTempGO();

            gameObj01.SetActive(true);

            InstObjCount = 0;
      
            //time
            GameTime = 0;
            TotalGameTime = 0;
            ShowAddCharEndTime = 0;

            //states
            GameState = true;
            IdleState = false;
            LooseState = false;
            EndGameState = false;

            GameScore = 0;
            LostScore = 0;

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
        }

    }

    private void MyDebug(string CallPoint)
    {
        string TMP = "";

        for (int i = 0; i < 20; i++)
        {
            TMP = TMP + "- ";
        }
        Debug.Log(TMP);

        Debug.Log("Call from: " + CallPoint);
        //Debug.Log("TotalGameTime" + TotalGameTime);

        Debug.Log("CycleLenght: " + CycleLenght);
        Debug.Log("CurrentGameCycle: " + CurrentGameCycle);

        Debug.Log("Game Score: " + GameScore);
        Debug.Log("Lost Score: " + LostScore);

        Debug.Log("IdleState- " + IdleState + "/ GameState-" + GameState + "/ LooseState-" + LooseState + "/ PauseState-" + PauseState + "/ EndGameState-" + EndGameState + "/ ShowAddChar-" + ShowAddChar + "/ CatchState-" + CatchState);

        TMP = "";

        //add next range values
        for (int i = 0; i < GameplayRanges.Count; i++)
        {
            TMP = TMP + ", " + GameplayRanges[i];
        }

        Debug.Log("GameplayRanges:" + TMP);
        TMP = "";

        //add speed A
        for (int i = 0; i < GameASpeed.Count; i++)
        {
            TMP = TMP + ", " + GameASpeed[i];
        }

        Debug.Log("GameASpeed: " + TMP);
        TMP = "";

        //add speed B
        for (int i = 0; i < GameBSpeed.Count; i++)
        {
            TMP = TMP + ", " + GameBSpeed[i];
        }

        Debug.Log("GameBSpeed: " + TMP);
        TMP = "";
    }

    void SpeedController()
    {

        if (InstObjCount < 25)
        {
            Time.timeScale = 1.0f;
        }

    }

}
