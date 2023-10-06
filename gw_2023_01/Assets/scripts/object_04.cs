using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class object_04 : MonoBehaviour
{

    private Component[] ObjectFrames;
    private int FrameN;
    private float ObjectTime;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();
        hideAllObj04();

        FrameN = 0;
    }


    public void hideAllObj04()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }


    IEnumerator objectRender()
    {

        ObjectFrames[this.FrameN].gameObject.SetActive(true);

        if (this.FrameN == 4 && GameManager.HeroDirection == 3)
        {
            GameManager.ObjectCollected = true;
            GameManager.ObjNameToDelete = this.name;
            //Debug.Log(this.name + " is 4");
        }

        if (this.FrameN == 5)
        {
            GameManager.ObjectCollected = false;
            GameManager.ObjNameToDelete = this.name;
        }

        if (this.FrameN > 0)
        {
            ObjectFrames[this.FrameN - 1].gameObject.SetActive(false);
        }

        yield return null;
    }

    private void OnDestroy()
    {
        //
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
