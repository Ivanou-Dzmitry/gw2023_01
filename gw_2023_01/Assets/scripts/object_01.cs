using System.Collections;
using UnityEngine;

public class object_01 : MonoBehaviour
{
    private Component[] ObjectFrames;
    private int FrameN;
    private float ObjectTime;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);

        this.FrameN = 0;
    }


    IEnumerator objectRender()
    {

        ObjectFrames[this.FrameN].gameObject.SetActive(true);

        //try to add score
        if (this.FrameN  == 4 && GameManager.HeroDirection == 0)
        {
            GameManager.ObjectCollected = true;
            GameManager.ObjNameToDelete = this.name;
            //Debug.Log(this.name + " is 4");
        }

        //lost object
        if (this.FrameN == 5)
        {
            GameManager.ObjectCollected = false;
            GameManager.ObjNameToDelete = this.name;
        }

        //turn on previus
        if (this.FrameN > 0)
        {
            ObjectFrames[this.FrameN -1].gameObject.SetActive(false);
        }

        yield return null;
    }


    void Update()
    {
        if (GameManager.GameState)
        {
            this.ObjectTime = this.ObjectTime + Time.unscaledDeltaTime;

            StartCoroutine(objectRender());

            if (this.ObjectTime > 1f)
            {
                this.FrameN++;
                this.ObjectTime = 0;
            }
        }
    }

}
