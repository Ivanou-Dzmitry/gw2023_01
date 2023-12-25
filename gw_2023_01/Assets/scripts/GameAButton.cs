using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameAButton : MonoBehaviour
{
    //GameManager GM;
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
        
        GM.gameA();
        spriteRenderer.sprite = mouseClicked;
    }

    private void OnMouseUp()
    {
        spriteRenderer.sprite = regular;
    }

}
