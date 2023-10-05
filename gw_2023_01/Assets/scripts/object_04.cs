using System.Collections.Generic;
using UnityEngine;


public class object_04 : MonoBehaviour
{

    private Component[] ObjectFrames;
    private int FrameN;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();
        hideAllObj04();
        InvokeRepeating("objectRender", 0f, 1f);
        FrameN = -1;

        Destroy(gameObject, 6f);
    }


    public void hideAllObj04()
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
            game_manager.FinalObjectFrame[3] = 3;
        }
        else
        {
            game_manager.FinalObjectFrame[3] = -1;
        }

        if (FrameN > 0)
        {
            ObjectFrames[FrameN - 1].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        game_manager.FinalObjectFrame[3] = -1;
    }

    void Update()
    {
        if (game_manager.HeroDirection == game_manager.FinalObjectFrame[3])
        {
            game_manager.GameScore++;
            Destroy(this);
        }
    }



}
