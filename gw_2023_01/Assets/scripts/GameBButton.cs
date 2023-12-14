using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameBButton : MonoBehaviour
{
    public GameManager GM;
    public SoundManager SM;
    public SpriteRenderer spriteRenderer;

    public Sprite regular;
    public Sprite mouseClicked;

    private void OnMouseDown()
    {
        if (!GameManager.GameState) { SM.ButtonClick(); }
        GM.gameB();
        spriteRenderer.sprite = mouseClicked;
    }

    private void OnMouseUp()
    {
        spriteRenderer.sprite = regular;
    }


}
