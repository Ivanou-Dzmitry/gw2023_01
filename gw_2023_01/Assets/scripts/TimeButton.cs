using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeButton : MonoBehaviour
{
    public GameManager GM;
    public SoundManager SM;
    public SpriteRenderer spriteRenderer;

    public Sprite regular;
    public Sprite mouseClicked;


    private void OnMouseDown()
    {
        //sound
        if (!GameManager.GameState)
            SM.ButtonClick();

        GM.idleState();
        
        spriteRenderer.sprite = mouseClicked;
    }

    private void OnMouseUp()
    {
        spriteRenderer.sprite = regular;
    }
}
