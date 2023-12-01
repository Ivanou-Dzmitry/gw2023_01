using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private Component[] ObjectFrames;
    private int FrameN;
    private float ObjectTime;

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

        if (!GameManager.soundQueue.Contains(this.ObjectSound))
        {
            GameManager.soundQueue.Add(this.ObjectSound);
        }

        //UnityEngine.Debug.Log("RENDER START");
    }


    void objectRender()
    {
        ObjectFrames[this.FrameN].gameObject.SetActive(true);
    }

    void counterLogic()
    {
        if (this.FrameN < 4)
        {
            if (!GameManager.soundQueue.Contains(this.ObjectSound))
            {
                GameManager.soundQueue.Add(this.ObjectSound);
            }
        }

        //try to add score
        if (this.FrameN == 4 && GameManager.HeroDirection == this.Direction)
        {
            //catch sound
            _clip = (AudioClip)Resources.Load("catch");
            SoundManager.Instance.PlaySound(_clip);

            GameManager.GameScore++;

            Destroy(this.gameObject);
        }

        if (this.FrameN == 4 && GameManager.HeroDirection != this.Direction && !GameManager.IdleState && !GameManager.PauseState)
        {
            GameManager.LooseState = true;

            if (GameManager.soundQueue.Count > 0)
            {
                GameManager.soundQueue.RemoveAt(GameManager.soundQueue.Count - 1);
            }

            //loose sound
            _clip = (AudioClip)Resources.Load("loose");
            SoundManager.Instance.PlaySound(_clip);

            if (GameManager.ShowAddChar)
            {
                GameManager.LostScore++; //if we see add char
            }
            else
            {
                GameManager.LostScore = GameManager.LostScore + 2; //if we cant see add shar
            }

            GameManager.LooseObjectName = this.name;
            
            //UnityEngine.Debug.Log("LOOSE!" + this.FrameN);
        }


        //last frame
        if (this.FrameN == 7 && !GameManager.IdleState)
        {
            GameManager.LooseState = false;

            if (GameManager.GameState)
            {
                if (GameManager.soundQueue.Count > 0)
                {
                    _clip = (AudioClip)Resources.Load(GameManager.soundQueue[0]);
                    SoundManager.Instance.PlaySound(_clip);
                }
            }

            Destroy(this.gameObject);
        }

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

    void objectLogic()
    {
        //time for object
        this.ObjectTime = this.ObjectTime + Time.unscaledDeltaTime;

        //render
        objectRender();

        //each 1sec
        if (this.ObjectTime > 1f)
        {            
            //if not pause
            if (!GameManager.PauseState)
            {
                //counterlogic
                counterLogic();

                //add frame only for loose
                if (GameManager.LooseState && GameManager.LooseObjectName == this.name)
                {
                    this.FrameN++;
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
