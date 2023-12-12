using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectController : MonoBehaviour
{
    private Component[] ObjectFrames;
    private int FrameN;
    private float ObjectTime;
    private string State;

    private AudioClip _clip;

    //direction of player for win
    [SerializeField] private int Direction;
    [SerializeField] private string ObjectSound;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);

        this.FrameN = 0;

        GameManager.soundQueue.Add(this.ObjectSound);
    }


    void objectRender()
    {
        ObjectFrames[this.FrameN].gameObject.SetActive(true);
    }

    void counterLogic()
    {
        //try to add score
        if (this.FrameN == 4 && GameManager.HeroDirection == this.Direction)
        {
            GameManager.GameScore++;
            Destroy(this.gameObject);
        }

        //to loose state
        if (this.FrameN == 4 && GameManager.HeroDirection != this.Direction && !GameManager.IdleState && !GameManager.PauseState)
        {
            GameManager.LooseState = true;

            if (GameManager.ShowAddChar)
            {
                GameManager.LostScore++; //if we see add char
            }
            else
            {
                GameManager.LostScore = GameManager.LostScore + 2; //if we cant see add shar
            }

            GameManager.LooseObjectName = this.name;
        }

        //last frame
        if (this.FrameN == 7 && !GameManager.IdleState)
        {
            GameManager.LooseState = false;
            Destroy(this.gameObject);
        }

        //destroy for Idle
        if (this.FrameN == 7 && GameManager.IdleState)
        {
            Destroy(this.gameObject);
        }
    }


    void hidePrevious()
    {
        //turn on previus
        if (this.FrameN > 0)
        {
            ObjectFrames[this.FrameN - 1].gameObject.SetActive(false);
        }
    }


    void Update()
    {
        objectLogic();
        //render
        
        //destroy other
        if (GameManager.EndGameState == true && GameManager.LooseObjectName != this.name)
        {
            hideObect();
            this.FrameN = 0;
            Destroy(this.gameObject);
        }

    }

    private void LateUpdate()
    {
        //Debug.Log("LATE");
    }


    private void StateChanger()
    {
        if (this.FrameN < 4)
        {
            State = "normal"; 
        }

        if (this.FrameN == 4 && GameManager.HeroDirection != this.Direction && !GameManager.IdleState && !GameManager.PauseState)
        {
            GameManager.LooseState = true;
            State = "loose";
        }

        if (this.FrameN == 4 && GameManager.HeroDirection == this.Direction)
        {
            State = "catch";
            //catch sound

        }

        if (this.FrameN == 7 && !GameManager.IdleState && GameManager.soundQueue.Count > 0)
        {
            State = "after_loose";
        }
    }

    private void SoundPlayer()
    {
        if (State == "catch")
        {
            _clip = (AudioClip)Resources.Load("catch");
            SoundManager.Instance.PlaySound(_clip);
        }

        if (State == "normal")
        {
            GameManager.soundQueue.Add(this.ObjectSound);
        }

        if (State == "after_loose")
        {
            _clip = (AudioClip)Resources.Load(GameManager.soundQueue.Last());
            SoundManager.Instance.PlaySound(_clip);
        }
    }

    void objectLogic()
    {
        StateChanger();

        objectRender();

        //time for object
        this.ObjectTime = this.ObjectTime + Time.unscaledDeltaTime;

            //each 1sec
            if (this.ObjectTime > 1f)
        {            
            //if not pause
            if (!GameManager.PauseState)
            {
                SoundPlayer();

                //counterlogic
                counterLogic();

                //add frame only for loose
                if (GameManager.LooseState && GameManager.LooseObjectName == this.name)
                {
                    this.FrameN++;
                }

                if (GameManager.LooseState && GameManager.LooseObjectName == this.name && this.FrameN == 5)
                {
                    _clip = (AudioClip)Resources.Load("loose");
                    SoundManager.Instance.PlaySound(_clip);
                }


                //add frames for all
                if (!GameManager.LooseState)
                {
                    this.FrameN++;
                }

                //hide previous
                hidePrevious();

                //zero time
                this.ObjectTime = 0;
            }

        }

    }

    void hideObect()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }

}
