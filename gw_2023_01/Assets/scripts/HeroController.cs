using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HeroController : MonoBehaviour
{
    private Component[] ObjectFrames;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        hideObect();
    }

    public void hideObect()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }

    void objectRender(int HeroDirection)
    {
        ObjectFrames[HeroDirection].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        ButtonInput();

        objectRender(GameManager.HeroDirection);
    }

    public void HeroPosition_01()
    {
        if (GameManager.GameState)
        {
            GameManager.HeroDirection = 0; //left top
        }

    }

    public void HeroPosition_02()
    {
        if (GameManager.GameState)
        {
            GameManager.HeroDirection = 3; //right top    
        }

    }

    public void HeroPosition_03()
    {
        if (GameManager.GameState)
        {
            GameManager.HeroDirection = 1; //left bottom
        }
    }

    public void HeroPosition_04()
    {
        if (GameManager.GameState)
        {
            GameManager.HeroDirection = 2; //right bottom
        }
    }


    void ButtonInput()
    {
        if (GameManager.GameState)
        {
            hideObect();

            //pos 1
            if (UserInput.instance.Button1Pressed)
            {
                HeroPosition_01();
            }

            //pos 2
            if (UserInput.instance.Button2Pressed)
            {
                HeroPosition_02();
            }

            //pos 3
            if (UserInput.instance.Button3Pressed)
            {
                HeroPosition_03();
            }

            //pos 4
            if (UserInput.instance.Button4Pressed)
            {
                HeroPosition_04();
            }

        }
            
    }
}
