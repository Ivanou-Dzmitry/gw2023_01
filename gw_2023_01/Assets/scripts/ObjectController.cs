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
        if (GameManager.GameState)
        {
            this.ObjectTime = this.ObjectTime + Time.unscaledDeltaTime;

            objectRender();

            if (this.ObjectTime > 1f)
            {
                StartCoroutine(counterLogic());

                this.FrameN++;

                hidePrevious();

                this.ObjectTime = 0;
            }
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(counterLogic());
    }

}
