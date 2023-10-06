using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    private Component[] ObjectFrames;
 
    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        hideAllHero01();
    }

    void hideAllHero01()
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

        if (GameManager.GameState)
        {
            objectRender(GameManager.HeroDirection);
        }
            
    }
}
