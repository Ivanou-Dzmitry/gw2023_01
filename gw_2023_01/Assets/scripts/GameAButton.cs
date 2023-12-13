using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameAButton : MonoBehaviour
{
    //GameManager GM;
    public GameManager GM;
    public SpriteRenderer spriteRenderer;

    public Sprite regular;
    public Sprite mouseClicked;

    private void OnMouseDown()
    {
        GM.gameA();
        spriteRenderer.sprite = mouseClicked;
    }

    private void OnMouseUp()
    {
        spriteRenderer.sprite = regular;
    }

}
