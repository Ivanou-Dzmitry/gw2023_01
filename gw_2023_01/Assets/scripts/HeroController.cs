using UnityEngine;
using UnityEngine.UI;


public class HeroController : MonoBehaviour
{
    private Component[] ObjectFrames;

    [SerializeField] private Button rtButton;
    [SerializeField] private Button rbButton;
    [SerializeField] private Button ltButton;
    [SerializeField] private Button lbButton;

    [SerializeField] private Sprite LargeButtonPressed;
    [SerializeField] private Sprite LargeButtonIdle;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        hideObect();
    }

    void hideObect()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }

    void objectRender(int FrameNumber)
    {

        ObjectFrames[FrameNumber].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        ButtonInput();

        hideObect();

        objectRender(GameManager.HeroDirection);
    }

    public void HeroLeftTop()
    {
        if (GameManager.GameState)
        {
            GameManager.HeroDirection = 0; //left top
            ltButton.image.sprite = LargeButtonPressed;
        }

    }

    public void HeroLeftBottom()
    {
        if (GameManager.GameState)
        {
            GameManager.HeroDirection = 1; //left bottom
            lbButton.image.sprite = LargeButtonPressed;
        }
    }

    public void HeroRightTop()
    {
        if (GameManager.GameState)
        {
            GameManager.HeroDirection = 3; //right top    
            rtButton.image.sprite = LargeButtonPressed;
        }

    }

    public void HeroRightBottom()
    {
        if (GameManager.GameState)
        {
            GameManager.HeroDirection = 2; //right bottom
            rbButton.image.sprite = LargeButtonPressed;
        }
    }


    void ButtonInput()
    {
        if (GameManager.GameState)
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
            
    }
}
