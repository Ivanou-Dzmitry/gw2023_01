using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_03 : MonoBehaviour
{
    private Component[] ObjectFrames;
    private int FrameN;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();
        hideAllObj03();
        InvokeRepeating("objectRender", 0f, 1f);
        FrameN = -1;

        Destroy(gameObject, 6f);
    }


    public void hideAllObj03()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }

    void objectRender()
    {
        FrameN++;

        ObjectFrames[FrameN].gameObject.SetActive(true);

        if (FrameN == 4)
        {
            game_manager.FinalObjectFrame[2] = 2;
        }
        else
        {
            game_manager.FinalObjectFrame[2] = -1;
        }

        if (FrameN > 0)
        {
            ObjectFrames[FrameN - 1].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        game_manager.FinalObjectFrame[2] = -1;
    }

    void Update()
    {
        if (game_manager.HeroDirection == game_manager.FinalObjectFrame[2])
        {
            game_manager.GameScore++;
            Destroy(this);
        }
    }
}
