using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCharController : MonoBehaviour
{
    private Component[] ObjectFrames;

    // Start is called before the first frame update
    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        HideObj();

        //char animation
        InvokeRepeating("objectRender", 0f, 2f);
    }

    void HideObj()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }

    void AddCharRenderLogic()
    {
        //2 frames
        if (GameManager.TotalGameTime % 3 == 0)
        {
            ObjectFrames[0].gameObject.SetActive(true);
        }
        else
        {
            ObjectFrames[1].gameObject.SetActive(true);
        }
    }

    void objectRender()
    {
        //Debug.Log("GT: " + GameManager.TotalGameTime +" / GS:" + GameManager.GameState);

        HideObj();

        if (GameManager.GameState == true && GameManager.ShowAddChar == true && GameManager.EndGameState == false)
        {
            AddCharRenderLogic();
        }

        if (GameManager.IdleState == true && GameManager.ShowAddChar == true)
        {
            AddCharRenderLogic();
        }
    }

}
