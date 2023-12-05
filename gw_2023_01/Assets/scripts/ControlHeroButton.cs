
using UnityEngine;

public class ControlHeroButton : MonoBehaviour
{
    //GameManager GM;
    public SpriteRenderer spriteRenderer;

    [SerializeField] private int ButtonDirection;

    public Sprite regular;
    public Sprite mouseClicked;

    public void OnMouseDown()
    {
        spriteRenderer.sprite = mouseClicked;

        if (GameManager.GameState)
        {
            GameManager.HeroDirection = ButtonDirection;
        }
        
    }

    public void OnMouseUp()
    {
        spriteRenderer.sprite = regular;
    }

}
