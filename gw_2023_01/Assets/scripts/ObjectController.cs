using System.Collections;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private Component[] ObjectFrames;
    private int FrameN;
    private float ObjectTime;

    //direction of player for win
    [SerializeField] private int Direction;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);

        this.FrameN = 0;
    }


    void objectRender()
    {
        ObjectFrames[this.FrameN].gameObject.SetActive(true);
    }

    IEnumerator counterLogic()
    {
        //try to add score
        if (this.FrameN == 4 && GameManager.HeroDirection == this.Direction)
        {
            GameManager.ObjectCollected = true;
            GameManager.ObjNameToDelete = this.name;
        }

        //lost object
        if (this.FrameN == 5)
        {
            GameManager.ObjectCollected = false;
            GameManager.ObjNameToDelete = this.name;
            GameManager.LooseObjectName = this.name;
            GameManager.LooseState = true;

            //Debug.Log("Loose"+GameManager.LooseObjectName);
        }

        //last frame
        if (this.FrameN == 7)
        {
            GameManager.LooseState = false;
        }

            yield return null;
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
        //for all
        if (GameManager.GameState == true && GameManager.LooseState == false)
        {
            objectLogic();
            //Debug.Log("Stop here 1");
        }

        //for loose
        if (GameManager.LooseState == true && GameManager.LooseObjectName == this.name)
        {
            objectLogic();
            //Debug.Log("Stop here 2");
        }

        if (GameManager.EndGameState == true)
        {
            hideObect();
            //zero time
            this.FrameN = 0;
            Destroy(this.gameObject);
        }

        if (GameManager.IdleState)
        {
            objectLogic();
            //Debug.Log("Stop here 1");
        }

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
            //counterlogic
            StartCoroutine(counterLogic());
            
            //addframe
            this.FrameN++;
           
            //hide previous
            hidePrevious();

            //zero time
            this.ObjectTime = 0;
        }

    }

    void hideObect()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        StopCoroutine(counterLogic());
    }

}
