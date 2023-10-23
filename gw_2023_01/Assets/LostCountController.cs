using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCountController : MonoBehaviour
{
    private Component[] ObjectFrames;


    // Start is called before the first frame update
    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();
        HideObj();
    }
    public void HideObj()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }


    void objectRender(int LostScore)
    {
        if (LostScore == 0)
        {
            ObjectFrames[0].gameObject.SetActive(false);
            ObjectFrames[1].gameObject.SetActive(false);
            ObjectFrames[2].gameObject.SetActive(false);
        }

        if (LostScore == 1)
        {
            if (GameManager.TotalGameTime % 2 == 0)
            {
                ObjectFrames[0].gameObject.SetActive(true);
            } else
            {
                ObjectFrames[0].gameObject.SetActive(false);
            }
        }

        if (LostScore >= 2)
        {
            ObjectFrames[0].gameObject.SetActive(true);
        }


        if (LostScore == 3)
        {
            if (GameManager.TotalGameTime % 2 == 0)
            {
                ObjectFrames[1].gameObject.SetActive(true);
            }
            else
            {
                ObjectFrames[1].gameObject.SetActive(false);
            }
        }

        if (LostScore >= 4)
        {
            ObjectFrames[1].gameObject.SetActive(true);
        }

        if (LostScore == 5)
        {
            if (GameManager.TotalGameTime % 2 == 0)
            {
                ObjectFrames[2].gameObject.SetActive(true);
            }
            else
            {
                ObjectFrames[2].gameObject.SetActive(false);
            }
        }

        if (LostScore >= 6 || GameManager.EndGameState == true)
        {
            ObjectFrames[2].gameObject.SetActive(true);
        }

 
    }
    // Update is called once per frame
    void Update()
    {
        objectRender(GameManager.LostScore); 
    }
}
