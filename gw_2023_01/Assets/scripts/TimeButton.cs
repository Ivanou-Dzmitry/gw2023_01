using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeButton : MonoBehaviour
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
        //GM.GetComponent<GameManager>().gameA();
        GM.idleState();
        spriteRenderer.sprite = mouseClicked;
    }

    private void OnMouseUp()
    {
        spriteRenderer.sprite = regular;
    }
}
