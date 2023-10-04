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

    public static int HeroDirection;
    //public static int FinalObjectFrame;
    private int GameScore = 0;

    private int GameObjectsCount = 4; //game objects in current game

    private List<int> objectsOrder = new List<int>(); //for random order
    public static List<int> FinalObjectFrame = new List<int>();

    private List<string> Object01Stack = new List<string>();
    private List<string> Object02Stack = new List<string>();
    private List<string> Object03Stack = new List<string>();
    private List<string> Object04Stack = new List<string>();

    void Start()
    {
        //for random order
        randomCycle();
        IdleState = true;

        counterText.text = "0";
        HeroDirection = 0;

        Application.targetFrameRate = 15;

        InvokeRepeating("GameLogic", 0f, 1f);
        InvokeRepeating("SecondActionTimer", 0f, 2f);
        InvokeRepeating("ThirdActionTimer", 0f, 4f);

        TotalGameTime = 0;

        for (int i = 0; i < 4; i++)
        {
            FinalObjectFrame.Insert(i, -1);
        }
    }



    void randomCycle()
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

    void SecondActionTimer()
    {
        SecondActionTime = TotalGameTime + 2;
    }

    void ThirdActionTimer()
    {
        ThirdActionTime = TotalGameTime + 4;
    }


    void GameLogic()
    {
        //Debug.Log(TotalGameTime +"/"+ SecondActionTime);

        TotalGameTime++;

        if (TotalGameTime == 2)
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

        if (TotalGameTime > 28 && TotalGameTime == SecondActionTime)
        {
            RandomObject = UnityEngine.Random.Range(1, 5);
            objectForRender(RandomObject);
            //Debug.Log("here: " + TotalGameTime + "/" + SecondActionTime);
        }

        ScoreCounter();

        for (int i = 0; i < 4; i++)
        {
            FinalObjectFrame.Insert(i, -1);
        }

        SpeedController();

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
        ButtonInput();

        //Debug.Log(HeroDirection +"/"+ FinalObjectFrame[0]);

        //ScoreCounter();

    }

    void ScoreCounter()
    {
        counterText.text = TotalGameTime + " S:" + GameScore.ToString() + " F:" + InstObjCount.ToString();
        
        //top left
        if (HeroDirection == FinalObjectFrame[0])
        {
            GameScore++;

            if (Object01Stack.Count > 0)
            {

                if (GameObject.Find(Object01Stack[0]) != null)
                {
                    GameObject GO2Del = GameObject.Find(Object01Stack[0]);
                    Destroy(GO2Del);
                    Debug.Log(Object01Stack[0] + " was Destroyed!");
                    Object01Stack.RemoveAt(0);
                }
            }

            

        }

        //bottom left
        if (HeroDirection == FinalObjectFrame[1])
        {
            GameScore++;

            if (Object01Stack.Count > 0)
            {

                if (GameObject.Find(Object02Stack[0]) != null)
                {
                    GameObject GO2Del = GameObject.Find(Object02Stack[0]);
                    Destroy(GO2Del);
                    Debug.Log(Object02Stack[0] + " was Destroyed!");
                    Object02Stack.RemoveAt(0);
                }
            }
        }

        //bottom right
        if (HeroDirection == FinalObjectFrame[2])
        {
            GameScore++;

            if (Object03Stack.Count > 0)
            {

                if (GameObject.Find(Object03Stack[0]) != null)
                {
                    GameObject GO2Del = GameObject.Find(Object03Stack[0]);
                    Destroy(GO2Del);
                    Debug.Log(Object03Stack[0] + " was Destroyed!");
                    Object03Stack.RemoveAt(0);
                }
            }
        }

        //top right
        if (HeroDirection == FinalObjectFrame[3])
        {
            GameScore++;

            if (Object04Stack.Count > 0)
            {

                if (GameObject.Find(Object04Stack[0]) != null)
                {
                    GameObject GO2Del = GameObject.Find(Object04Stack[0]);
                    Destroy(GO2Del);
                    Debug.Log(Object04Stack[0] + " was Destroyed!");
                    Object04Stack.RemoveAt(0);
                }
            }
        }


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
    }

    void SpeedController()
    {

        if (InstObjCount < 25)
        {
            Time.timeScale = 1.0f;
            Debug.Log("Speed 1");
        }

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

    }
}
