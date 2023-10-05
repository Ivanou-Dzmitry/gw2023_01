using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_01 : MonoBehaviour
{
    private Component[] ObjectFrames;
    private int FrameN;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);

        InvokeRepeating("objectRender", 0f, 1f);

        FrameN = -1;

        Destroy(gameObject, 6f);
    }


    void objectRender()
    {
        FrameN++;

        ObjectFrames[FrameN].gameObject.SetActive(true);

        if (FrameN  == 4)
        {
            game_manager.FinalObjectFrame[0] = 0;
        }
        else
        {
            game_manager.FinalObjectFrame[0] = -1;
        }

        if (FrameN > 0)
        {
            ObjectFrames[FrameN-1].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        game_manager.FinalObjectFrame[0] = -1;
    }


    void Update()
    {
        if (game_manager.HeroDirection == game_manager.FinalObjectFrame[0])
        {
            game_manager.GameScore++;
            Destroy(this);
            
        }
    }

}
