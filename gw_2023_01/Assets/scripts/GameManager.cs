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
    public static int GameScore;    //Score
    public static int LostScore;

    public static int TotalGameTime;
    public static int HeroDirection; //player direction

    private float GameTime;
    public static float GameSpeed = 1;
    private float LooseSpeed = 1;
    private float CurrentSpeed;

    private int GameCycles = 10;
    private int GameCycle = 0;
    private int CycleLenght = 40;

    //initial Gameplay Ranges
    //List<int> GameplayRanges = new List<int> { 25, 50, 75, 100};
    List<int> GameplayRanges = new List<int> { 10, 20, 30, 40 };

    List<float> GameASpeed = new List<float> { 1.0f, 0.95f, 0.85f, 0.55f, 0.5f };
    List<float> GameBSpeed = new List<float> { 0.6f, 0.55f, 0.5f, 0.45f, 0.4f };

    private readonly int GameObjectsCount = 4; //game objects in current game

    private readonly List<int> objectsOrder = new List<int>(); //for random order

    string[] ObjectSounds = { "tone01", "tone02", "tone03", "tone04" };

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
        if (TotalGameTime < 23 && GameCycle == 0)
        {
            IntroGameplay();
        }

        Debug.Log(GameScore);
        //random every odd sec

        //1 fish 4sec
        if (TotalGameTime > 23 | GameCycle > 0)
        {
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

            //1 fish 3sec
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

            //1 fish 2sec
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
                _clip = (AudioClip)Resources.Load(ObjectSounds[0]);
                SoundManager.Instance.PlaySound(_clip);
            }

            if (forsound.Contains("obj_02"))
            {
                _clip = (AudioClip)Resources.Load(ObjectSounds[1]);
                SoundManager.Instance.PlaySound(_clip);
            }

            if (forsound.Contains("obj_03"))
            {
                _clip = (AudioClip)Resources.Load(ObjectSounds[2]);
                SoundManager.Instance.PlaySound(_clip);
            }

            if (forsound.Contains("obj_04"))
            {
                _clip = (AudioClip)Resources.Load(ObjectSounds[3]);
                SoundManager.Instance.PlaySound(_clip);
            }

        }

        if (ObjectsCount > 1 && !LooseState)
        {
            //play random sound
            int RandomSoundNumber = UnityEngine.Random.Range((int)0, (int)4);
            _clip = (AudioClip)Resources.Load(ObjectSounds[RandomSoundNumber]);
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

    public void ScoreCounter()
    {
         ScoreOutput();

        if (LostScore > 0)
        {
            UIManager.ShowMissText = true;
        }

        //game cycles 100, 200, 300 etc
        if (GameScore > 0 && GameScore % CycleLenght == 0)
        {
            GameCycle++;

            GameplayRanges[0] = GameplayRanges[0] + CycleLenght;
            GameplayRanges[1] = GameplayRanges[1] + CycleLenght;
            GameplayRanges[2] = GameplayRanges[2] + CycleLenght;
            GameplayRanges[3] = GameplayRanges[3] + CycleLenght;

            Debug.Log(GameplayRanges[0] + "/" + GameplayRanges[1] + "/" + GameplayRanges[2] + "/" + GameplayRanges[3]);
        }

        //game loop
        if (GameCycle == GameCycles)
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

    void SpeedController()
    {

        if (InstObjCount < 25)
        {
            Time.timeScale = 1.0f;
        }

    }

}
