using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameBButton : MonoBehaviour
{
    public GameManager GM;
    public SpriteRenderer spriteRenderer;

    public Sprite regular;
    public Sprite mouseClicked;

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
        GM.gameB();
        spriteRenderer.sprite = mouseClicked;
    }

    private void OnMouseUp()
    {
        spriteRenderer.sprite = regular;
    }


}
