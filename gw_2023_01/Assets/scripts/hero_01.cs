using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hero_01 : MonoBehaviour
{
    private Component[] ObjectFrames;
    private int FrameN;

    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();
        hideAllHero01();
    }

    public void hideAllHero01()
    {
        foreach (SpriteRenderer sprite in ObjectFrames)
            sprite.gameObject.SetActive(false);
    }

    void objectRender(int FrameNumber)
    {

        ObjectFrames[FrameNumber].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        hideAllHero01();
        objectRender(game_manager.HeroDirection);
    }
}
