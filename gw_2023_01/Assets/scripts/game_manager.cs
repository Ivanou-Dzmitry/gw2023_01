using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class game_manager : MonoBehaviour
{
    public GameObject gameObj01;
    public GameObject gameObj02;
    public GameObject gameObj03;
    public GameObject gameObj04;

    public TMP_Text counterText;

    GameObject TempGO01, TempGO02, TempGO03, TempGO04;

    private bool IdleState, GameState, LooseState, PauseState;

    private int TotalGameTime, RandomObject, SecondActionTime, ThirdActionTime, InstObjCount;

    private float GameTime;

    public static int HeroDirection;
    //public static int FinalObjectFrame;
    public static int GameScore = 0;

    private int GameObjectsCount = 4; //game objects in current game

    private List<int> objectsOrder = new List<int>(); //for random order
    public static List<int> FinalObjectFrame = new List<int>();

    public static List<string> Object01Stack = new List<string>();
    private List<string> Object02Stack = new List<string>();
    private List<string> Object03Stack = new List<string>();
    private List<string> Object04Stack = new List<string>();

    void Start()
    {
        //for random order
        randomGameObjOrder();

        //idle sate of game
        IdleState = true;

        counterText.text = "0"; //start count

        HeroDirection = 2; // bottom right

        Application.targetFrameRate = 15;

        TotalGameTime = 0;

        for (int i = 0; i < 4; i++)
        {
            FinalObjectFrame.Insert(i, -1);
        }
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


    void GameLogic()
    {
  
        if (TotalGameTime == 1)
        {
            objectForRender(objectsOrder[0]);
        }

        if (TotalGameTime == 9)
        {
            objectForRender(objectsOrder[1]);
        }

        if (TotalGameTime == 16)
        {
            objectForRender(objectsOrder[2]);
        }

        if (TotalGameTime == 23)
        {
            objectForRender(objectsOrder[3]);
        }

        if (TotalGameTime > 28 && TotalGameTime%2 == 0)
        {
            RandomObject = UnityEngine.Random.Range(1, 5);
            objectForRender(RandomObject);
            //Debug.Log("here: " + TotalGameTime + "/" + SecondActionTime);
        }

        /*
        if (TotalGameTime > 30 && TotalGameTime == ThirdActionTime)
        {
            RandomObject = UnityEngine.Random.Range(1, 5);
            objectForRender(RandomObject);
            //Debug.Log(ThirdActionTime);
        }*/

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
                Object01Stack.Add(TempName);

                //Debug.Log(TempName + " was created!");

                break;
            case 2:
                
                TempName = "object2_" + InstObjCount.ToString();

                TempGO02 = Instantiate(gameObj02);
                TempGO02.name = TempName;

                Object02Stack.Add(TempName);
                break;
            case 3:
                
                TempName = "object3_" + InstObjCount.ToString();

                TempGO03 = Instantiate(gameObj03);
                TempGO03.name = TempName;
                Object03Stack.Add(TempName);

                break;
            case 4:
                
                TempName = "object4_" + InstObjCount.ToString();

                TempGO04 = Instantiate(gameObj04);
                TempGO04.name = TempName;
                Object04Stack.Add(TempName);

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

            ScoreCounter();

            if (GameTime > 1f)
            {
                //Debug.Log("TotalGameTime: " + TotalGameTime);
                //Debug.Log("GameTime: " + GameTime);

                Debug.Log(FinalObjectFrame[0] + " : "+ FinalObjectFrame[1] + " : " + FinalObjectFrame[2] + " : " + FinalObjectFrame[3]);
                
                GameLogic();
                TotalGameTime++;
                GameTime = 0;
            }

            
        }
        

        //Debug.Log(HeroDirection +"/"+ FinalObjectFrame[0]);

    }

    void ScoreCounter()
    {
        //TotalGameTime + 
        counterText.text = GameScore.ToString() + "/ " + InstObjCount.ToString();

    }

    void ButtonInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            HeroDirection = 0; //left top
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            HeroDirection = 1; //left bottom
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            HeroDirection = 2; //right bottom
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            HeroDirection = 3; //right top
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameState = true; 
            IdleState = false;
            Debug.Log("Game Start");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseState = true; 
            GameState = false;
            Debug.Log("Game Paused");
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
