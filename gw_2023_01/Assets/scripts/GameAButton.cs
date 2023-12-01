using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameAButton : MonoBehaviour
{
    //GameManager GM;
    public SpriteRenderer spriteRenderer;

    public Sprite regular;
    public Sprite mouseClicked;

    public GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
